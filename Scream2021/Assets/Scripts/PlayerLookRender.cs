using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookRender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer) renderer.enabled = false;
    }
}
