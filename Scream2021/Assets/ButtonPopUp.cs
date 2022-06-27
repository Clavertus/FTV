using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPopUp : MonoBehaviour
{
    [SerializeField] float delayPopUpInSec = 1f;
    [SerializeField] float popUpSpeed = 200f;
    [SerializeField] float offsetY = -200f;

    bool startPopUp = false;
    float targetYPosition = 0f;
    RectTransform myRectTransform = null;
    IEnumerator Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        targetYPosition = myRectTransform.position.y;
        myRectTransform.position = new Vector3(
            myRectTransform.position.x,
            myRectTransform.position.y + offsetY,
            myRectTransform.position.z
            );
        startPopUp = false;
        yield return new WaitForSeconds(delayPopUpInSec);
        startPopUp = true;
    }

    private void Update()
    {
        if(startPopUp)
        {
            if(myRectTransform.position.y < targetYPosition)
            {
                myRectTransform.position = new Vector3(
                    myRectTransform.position.x,
                    myRectTransform.position.y + popUpSpeed * Time.deltaTime,
                    myRectTransform.position.z
                    );
            }
            else
            {
                startPopUp = false;
            }
        }
    }
}
