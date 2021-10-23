using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSideDoor : MonoBehaviour
{
    [SerializeField] float pushDistance;
    [SerializeField] float pushDelay = 1;
    [SerializeField] GameObject doorSelectable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doorSelectable.tag == ("Selected")) { PushDoor(); }
    }
    void PushDoor()
    {
        Debug.Log("push"); 
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - pushDistance);
        doorSelectable.tag = ("Untagged");
        StartCoroutine(makeSelectableAgain()); 
    }
    private IEnumerator makeSelectableAgain()
    {
        yield return new WaitForSeconds(pushDelay);
        doorSelectable.tag = ("Selectable"); 
    }
}
