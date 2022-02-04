using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualisePoint : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] Color gizmoColor = Color.white;
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, 1);
    }
#endif
}
