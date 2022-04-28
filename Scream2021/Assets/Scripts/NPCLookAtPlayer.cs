using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookAtPlayer : MonoBehaviour
{
    [SerializeField] PlayNPCDialog playNPCDialogInst = null;
    [SerializeField] Transform playerLookAt = null;
    [SerializeField] Transform headBone = null;
    [SerializeField] float distanceToLookAt = 5f; 
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] [Range(0,180)] float angleCheck = 90f;
    bool lookAt = false;

    void Start()
    {
        lookAt = false;
    }

    // Start is called before the first frame update
    void Update()
    {
        Vector3 directionToTarget = transform.position - playerLookAt.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        lookAt = false;
        //Debug.Log(angle);
        //Debug.Log(lookAt);
        if (Mathf.Abs(angle) > 180 - angleCheck)
        {
            if (Vector3.Distance(transform.position, playerLookAt.position) <= distanceToLookAt)
            {
                lookAt = true;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (lookAt)
        {
            headBone.LookAt(playerLookAt);
        }
    }

    public Transform GetLookAtPoint()
    {
        return headBone;
    }
}
