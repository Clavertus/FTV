using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TrainLampsCombiner : MonoBehaviour
{
    /**
     * Note: this module brings best result if the Object of it is placed on origin of the World (0,0,0)
     * Perhaps there is a way to fix it ...
     * IDEA: technically we need to create a new object from RAW
     * in the for loop each object should be moved to corresponding postion in the origin of the world ...
     * Then we combined it to the new mesh
     * After loop whole mesh is moved to previous location of combined objects... (0,0,0) of parent?
     */
    [SerializeField] Transform ParentWhereToLookForLamps = null;

    [SerializeField] MeshFilter LampNewMesh = null;
    [SerializeField] MeshFilter BasesNewMesh = null;

    [SerializeField] bool enableLampCombine = false;
    [SerializeField] bool enableBaseCombine = false;

    void Start()
    {
        if (enableLampCombine)
        {
            CombineLamps();
        }
        if (enableBaseCombine)
        {
            CombineBases();
        }
    }

    public void CombineLamps()
    {
        //save rotation and position of a our object
        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        //set to 00 world positions for right objects aligning
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        TrainLampObject[] trainLamps = ParentWhereToLookForLamps.GetComponentsInChildren<TrainLampObject>();
        //get all mesh filters in children
        MeshFilter[] lampFilter = new MeshFilter[trainLamps.Length];
        for (int ix = 0; ix < trainLamps.Length; ix++)
        {
            lampFilter[ix] = trainLamps[ix].GetComponent<MeshFilter>();
        }

        Debug.Log(name + " is combining " + lampFilter.Length + " meshes!");

        //mesh that represent combined mesh
        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[lampFilter.Length];

        for (int ix = 0; ix < lampFilter.Length; ix++)
        {
            if (lampFilter[ix].transform == transform)
            {
                //remove our "parent" mesh filter from loop
                continue;
            }

            //take mesh from filter and apply transform of filter to combiner instance
            combiners[ix].subMeshIndex = 0;
            combiners[ix].mesh = lampFilter[ix].sharedMesh;
            combiners[ix].transform = lampFilter[ix].transform.localToWorldMatrix;
            //deactivate original object
            lampFilter[ix].gameObject.SetActive(false);
        }

        //combine meshes
        finalMesh.CombineMeshes(combiners);

        LampNewMesh.sharedMesh = finalMesh;

        //return of old rotation and position (rotation first as it can change position)
        transform.rotation = oldRot;
        transform.position = oldPos;
    }
    public void CombineBases()
    {
        //save rotation and position of a our object
        Quaternion oldRot = transform.rotation;
        Vector3 oldPos = transform.position;

        //set to 00 world positions for right objects aligning
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        TrainLampBaseObject[] trainBases = ParentWhereToLookForLamps.GetComponentsInChildren<TrainLampBaseObject>();
        //get all mesh filters in children
        MeshFilter[] lampFilter = new MeshFilter[trainBases.Length];
        for (int ix = 0; ix < trainBases.Length; ix++)
        {
            lampFilter[ix] = trainBases[ix].GetComponent<MeshFilter>();
        }

        Debug.Log(name + " is combining " + lampFilter.Length + " meshes!");

        //mesh that represent combined mesh
        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[lampFilter.Length];

        for (int ix = 0; ix < lampFilter.Length; ix++)
        {
            if (lampFilter[ix].transform == transform)
            {
                //remove our "parent" mesh filter from loop
                continue;
            }

            //take mesh from filter and apply transform of filter to combiner instance
            combiners[ix].subMeshIndex = 0;
            combiners[ix].mesh = lampFilter[ix].sharedMesh;
            combiners[ix].transform = lampFilter[ix].transform.localToWorldMatrix;
            //deactivate original object
            lampFilter[ix].gameObject.SetActive(false);
        }

        //combine meshes
        finalMesh.CombineMeshes(combiners);

        BasesNewMesh.sharedMesh = finalMesh;

        //return of old rotation and position (rotation first as it can change position)
        transform.rotation = oldRot;
        transform.position = oldPos;
    }
}
