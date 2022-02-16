using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacketTriggerZone : MonoBehaviour
{
    [SerializeField] TrainEffectController train = null;
    [SerializeField] GameObject jacketMemento = null;

    bool triggerOnce = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other)
        {
            if(other.tag == "Player")
            {
                GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(TriggerJacketAndLight());
            }
        }
    }
    public IEnumerator TriggerJacketAndLight()
    {
        yield return new WaitForSeconds(.25f);
        yield return StartCoroutine(train.SetLightOff());
        yield return new WaitForSeconds(3f);
        jacketMemento.SetActive(true);
        yield return StartCoroutine(train.SetLightOn());
    }
}
