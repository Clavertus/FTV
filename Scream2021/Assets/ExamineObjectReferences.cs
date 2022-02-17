using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObjectReferences : MonoBehaviour
{
    [SerializeField] Renderer mainObjRenderer = null;
    [SerializeField] Renderer smallObjRenderer = null;
    // Start is called before the first frame update
    void Start()
    {
        if (!mainObjRenderer) Debug.LogError("No renderer found!");
        if (!smallObjRenderer) Debug.LogError("No renderer found!");
    }

    public Renderer GetMainObjRenderer()
    {
        return mainObjRenderer;
    }
    public Renderer GetSmallObjRenderer()
    {
        return smallObjRenderer;
    }
}
