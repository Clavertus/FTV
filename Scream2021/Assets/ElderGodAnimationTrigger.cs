using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderGodAnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] bool openOnAwake = false;
    [SerializeField] bool openSlow = true;
    [SerializeField] bool idleOnAwake = true;

    // Start is called before the first frame update
    void Start()
    {
        if (idleOnAwake)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            if (openOnAwake)
            {
                if(openSlow)
                {
                    animator.SetTrigger("OpenSlow");
                }
                else
                {
                    animator.SetTrigger("Open");
                }
            }
        }
    }

}
