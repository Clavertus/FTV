using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookAtPlayer : MonoBehaviour
{
    [SerializeField] Transform playerLookAt = null;
    [SerializeField] Transform headBone = null;
    [SerializeField] float distanceToLookAt = 5f;
    bool lookAt = false;

    void Start()
    {
        lookAt = false;
    }

    // Start is called before the first frame update
    void Update()
    {
        if(Vector3.Distance(transform.position, playerLookAt.position) <= distanceToLookAt)
        {
            lookAt = true;
        }
        else
        {
            lookAt = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(lookAt) headBone.LookAt(playerLookAt);
    }

    public Transform GetLookAtPoint()
    {
        return headBone;
    }
}
