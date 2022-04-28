using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderGodAnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] bool openOnAwake = false;
    [SerializeField] bool openSlow = true;
    [SerializeField] bool idleOnAwake = true;
    [SerializeField] GameObject explosion = null;
    [SerializeField] GameObject trainToDissapear = null;
    [SerializeField] GameObject elderFodObjectToDissapear = null;


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


    public void TriggerClose()
    {
        StartCoroutine(AnimateDissapear());
    }

    private IEnumerator AnimateDissapear()
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.15f);
        if (explosion) explosion.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        trainToDissapear.SetActive(false);

        if(FindObjectOfType<Tara_Behaviour>()) FindObjectOfType<Tara_Behaviour>().elderGodDissapeared = true;
        elderFodObjectToDissapear.SetActive(false);
    }
}
