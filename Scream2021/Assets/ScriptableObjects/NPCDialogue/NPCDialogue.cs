using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FTV.Dialog
{
    [CreateAssetMenu(fileName = "Piece of Dialogue", menuName = "ScriptableObjects/NPCDialogue", order = 1)]
    public class NPCDialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogNode> nodes = new List<DialogNode>();

        Dictionary<string, DialogNode> nodeLookup = new Dictionary<string, DialogNode>();

        private void Awake()
        {

        }

        private void OnValidate()
        {

            nodeLookup.Clear();
            foreach(DialogNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogNode> GetAllChildren(DialogNode parentNode)
        {
            //Good IEnumerable example
            //List<DialogNode> result = new List<DialogNode>();
            foreach (string childId in parentNode.GetChildren())
            {
                if(nodeLookup.ContainsKey(childId))
                {
                    yield return nodeLookup[childId];//result.Add(nodeLookup[childId]);
                }
            }

            //return result;
        }
        public DialogNode GetSpecificChildren(DialogNode parentNode, string childrenKey)
        {
            //Good IEnumerable example
            //List<DialogNode> result = new List<DialogNode>();
            foreach (string childId in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childId) && (childId == childrenKey))
                {
                    return nodeLookup[childId];
                }
            }

            return null;
        }

#if UNITY_EDITOR
        public void CreateNode(DialogNode parent)
        {
            DialogNode newChildNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newChildNode, "Added Dialog Node");
            Undo.RecordObject(this, "Create Dialog Node");
            AddNode(newChildNode);
        }

        public void DeleteNode(DialogNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialog Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogNode nodeToDelete)
        {
            foreach (DialogNode node in GetAllNodes())
            {
                node.RemoveChildren(nodeToDelete.name);
            }
        }

        private void AddNode(DialogNode newChildNode)
        {
            nodes.Add(newChildNode);
            OnValidate();
        }

        private static DialogNode MakeNode(DialogNode parent)
        {
            DialogNode newChildNode = CreateInstance<DialogNode>(); //new DialogNode();

            newChildNode.name = System.Guid.NewGuid().ToString();
            newChildNode.SetText("Enter dialogue text to display");
            if (parent != null)
            {
                bool isParentPlayerSpeaker = parent.GetIsPlayerSpeaking();

                newChildNode.SetIsPlayerSpeaking(!isParentPlayerSpeaker);

                newChildNode.SetPosition(
                    new Vector2(parent.GetRect().position.x + 300, parent.GetRect().position.y + 50));
                parent.AddChildren(newChildNode.name);
            }

            return newChildNode;
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogNode newChildNode = MakeNode(null);
                AddNode(newChildNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach(DialogNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            //Unused but needed for interface
#endif
        }
    }
}
