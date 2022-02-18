using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [SerializeField] Material[] materialList = null;

    MeshRenderer meshRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        if(materialList.Length == 0)
        {
            Debug.LogWarning("No materials added to the MaterialSwitcher");
        }
        else
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
    }

    public void changeMaterial(int id)
    {
        if(id >= 0 && id < materialList.Length)
        {
            if(meshRenderer)
            {
                meshRenderer.sharedMaterial = materialList[id];
            }
        }
    }
}
