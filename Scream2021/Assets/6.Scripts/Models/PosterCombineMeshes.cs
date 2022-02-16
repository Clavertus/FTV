using UnityEngine;
using System.Collections;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PosterCombineMeshes : MonoBehaviour
{
    [SerializeField] bool enableCombine = false;
    void Start()
    {
        if(enableCombine)
        {
            CombineMeshes();
        }
    }

    public void CombineMeshes()
    {
        //save rotation and position of a house
        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        //set to 00 world positions for right objects aligning
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        //get all mesh filters in children
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Debug.Log(name + " is combining " + filters.Length + " meshes!");

        //mesh that represent combined mesh
        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for(int ix = 0; ix < filters.Length; ix++)
        {
            if(filters[ix].transform == transform)
            {
                //remove our "parent" mesh filter from loop
                continue;
            }

            //take mesh from filter and apply transform of filter to combiner instance
            combiners[ix].subMeshIndex = 0;
            combiners[ix].mesh = filters[ix].sharedMesh;
            combiners[ix].transform = filters[ix].transform.localToWorldMatrix;
            //deactivate original object
            filters[ix].gameObject.SetActive(false);
        }

        //combine meshes
        finalMesh.CombineMeshes(combiners);

        GetComponent<MeshFilter>().sharedMesh = finalMesh;

        //return of old rotation and position (rotation first as it can change position)
        transform.rotation = oldRot;
        transform.position = oldPos;
    }
}