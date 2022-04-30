using FTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoving : MonoBehaviour, ISaveable
{
    NavMeshAgent agent = null;
    Transform target = null;
    bool sitTarget = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Transform newTarget, bool sitOnEnd)
    {
        agent.updateRotation = true;
        agent.updatePosition = true;
        sitTarget = sitOnEnd;
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
                    if(sitTarget)
                    {
                        agent.updatePosition = false;
                    }
                    else
                    {
                        StartCoroutine(restoreNavMeshAgent());
                    }
                }
            }
        }
    }

    public IEnumerator restoreNavMeshAgent()
    {
        yield return new WaitForSeconds(.1f);
        agent.updateRotation = true;
        agent.updatePosition = true;
    }

    public bool IsMoving()
    {
        return agent.velocity.magnitude >= 0.05f;
    }

    public bool IsSitTarget()
    {
        return sitTarget;
    }

    #region REGION_SAVING

    [System.Serializable]
    struct SaveData
    {
        public bool sitTarget;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        data.sitTarget = sitTarget;
        return data;
    }

    public void RestoreState(object state)
    {
        SaveData data = (SaveData)state;
        sitTarget = data.sitTarget;
    }
    #endregion
}
