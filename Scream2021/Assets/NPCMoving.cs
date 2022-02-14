using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoving : MonoBehaviour
{
    NavMeshAgent agent = null;
    Transform target = null;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Transform newTarget)
    {
        target = newTarget;
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(target.position, path))
        {
            agent.SetDestination(target.position);
        }
        else
        {
            Debug.LogError("Can not calculate path to that position");
        }
    }

    public void Update()
    {
        if(target != null)
        {
            if (Vector3.Distance(transform.position, target.position) <= 1f)
            {
                if(agent.velocity.magnitude >= Mathf.Epsilon)
                {
                    return;
                }

                if (transform.rotation != target.rotation)
                {
                    agent.updateRotation = false;
                    //rotate to same angle as target
                    transform.rotation = target.rotation;
                    target = null;
                    StartCoroutine(restoreNavMeshAgent());
                }
            }
        }
    }

    private IEnumerator restoreNavMeshAgent()
    {
        yield return new WaitForSeconds(.1f);
        agent.updateRotation = true;
    }

    public bool IsMoving()
    {
        return agent.velocity.magnitude >= 0.1f;
    }
}
