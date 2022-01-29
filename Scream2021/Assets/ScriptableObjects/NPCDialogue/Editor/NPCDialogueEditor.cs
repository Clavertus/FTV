using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace FTV.Dialog.Editor
{
    public class NPCDialogueEditor : EditorWindow
    {
        NPCDialogue selectedDialogue = null;
        DialogStyle selectedDialogueStyle = null;
        [NonSerialized]
        GUIStyle defaultNodeStyle = null;
        [NonSerialized]
        GUIStyle playerNodeStyle = null;
        [NonSerialized]
        DialogNode draggingNode = null;
        [NonSerialized]
        private Vector2 draggingOffset;
        [NonSerialized]
        DialogNode creatingNode = null;
        [NonSerialized]
        DialogNode deletingNode = null;
        [NonSerialized]
        DialogNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;


        const float canvasSize = 4000;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(NPCDialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            NPCDialogue dialogueObject = EditorUtility.InstanceIDToObject(instanceID) as NPCDialogue;
            if (dialogueObject != null)
            {
                //selectedDialogue = dialogueObject;
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            defaultNodeStyle = new GUIStyle();
            defaultNodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            defaultNodeStyle.normal.textColor = Color.white;
            defaultNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            defaultNodeStyle.border = new RectOffset(12, 12, 12, 12);

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.normal.textColor = Color.white;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            NPCDialogue newDialogue = Selection.activeObject as NPCDialogue;
            if(newDialogue != null)
            {
                selectedDialogueStyle = null;
                selectedDialogue = newDialogue;
                Repaint();
            }
            else
            {
                //Debug.Log("Not a diallogue");
            }

            DialogStyle newDialogStyle = Selection.activeObject as DialogStyle;
            if (newDialogStyle != null)
            {
                selectedDialogueStyle = newDialogStyle;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialog selected");
            }
            else
            {
                ProcessEvents();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundTex = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0,0, canvasSize / 50, canvasSize / 50);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

                foreach (DialogNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                    //EditorUtility.SetDirty(selectedDialogue);
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                    //EditorUtility.SetDirty(selectedDialogue);
                }
            }
        }


        private void ProcessEvents()
        {
            if((Event.current.type == EventType.MouseDown) && (draggingNode == null))
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if(draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    //Record dragOffset and dragging
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if ((Event.current.type == EventType.MouseDrag) && (draggingNode != null))
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

                GUI.changed = true;
            }
            else if ((Event.current.type == EventType.MouseDrag) && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }
            else if((Event.current.type == EventType.MouseUp) && (draggingNode != null))
            {
                DialogNode nodeAtPoint = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if(nodeAtPoint != null)
                {
                    if(selectedDialogueStyle != null)
                    {
                        nodeAtPoint.SetDialogStyle(selectedDialogueStyle);
                        GUI.changed = true;
                    }
                }
                selectedDialogueStyle = null;
                draggingNode = null;
                draggingCanvas = false;
            }
        }

        public GameObject obj = null;
        private void DrawNode(DialogNode node)
        {
            GUIStyle style = defaultNodeStyle;
            if(node.GetIsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }
            GUILayout.BeginArea(node.GetRect(), style);

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("x"))
            {
                deletingNode = node;
            }
            DrawLinkButtons(node);
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }
            GUILayout.EndHorizontal();

            if(node.GetDialogStyle())
            {
                EditorGUILayout.LabelField(node.GetDialogStyle().name);
            }
            else
            {
                GUIStyle labelStyle = new GUIStyle();
                labelStyle.normal.textColor = Color.red;
                EditorGUILayout.LabelField("Missing dialog style!", labelStyle);
            }

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode != node)
            {
                if (linkingParentNode.GetChildren().Contains(node.name))
                {
                    if (GUILayout.Button("unlink"))
                    {
                        linkingParentNode.RemoveChildren(node.name);
                        linkingParentNode = null;
                    }
                }
                else
                {
                    if (GUILayout.Button("child"))
                    {
                        linkingParentNode.AddChildren(node.name);
                        linkingParentNode = null;
                    }
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                    {
                        linkingParentNode = null;
                    }
            }
        }

        private void DrawConnections(DialogNode node)
        {
            Vector3 startPosition =
                new Vector3(node.GetRect().xMax, node.GetRect().center.y, 0);
            foreach (DialogNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = 
                    new Vector3(childNode.GetRect().xMin, childNode.GetRect().center.y, 0);
                Vector3 startTangent = 
                    new Vector3((node.GetRect().xMax + childNode.GetRect().xMin) / 2, node.GetRect().center.y, 0);
                Vector3 endTangent =
                    new Vector3((node.GetRect().xMax + childNode.GetRect().xMin) / 2, childNode.GetRect().center.y, 0);

                Handles.DrawBezier
                    (
                    startPosition,
                    endPosition,
                    startTangent,
                    endTangent,
                    Color.white,
                    null,
                    4f
                    );
            }
        }

        private DialogNode GetNodeAtPoint(Vector2 point)
        {
            DialogNode upperNode = null;
            foreach (DialogNode node in selectedDialogue.GetAllNodes())
            {
                if(node.GetRect().Contains(point))
                {
                    upperNode = node;
                }
            }

            return upperNode;
        }
    }
}
