using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraChanger : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject train;
    public GameObject door1;
    public GameObject door2;
    public float distance;
    public float speed;
    public CanvasGroup image;

    public float fadeSpeed = 0.01f;
    public float duration = 3f;

    private bool moveDoors;
    private bool moveCam;
    private Vector3 finalDest;

    private void Start()
    {
        StartCoroutine(ManageTransitions());
        finalDest = new Vector3(door1.transform.localPosition.x - distance,0,0);
    }

    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        Vector3 position1 = door1.transform.localPosition;
        Vector3 newDest1 = new Vector3(position1.x - distance, position1.y, position1.z);

        Vector3 position2 = door2.transform.localPosition;
        Vector3 newDest2 = new Vector3(position2.x + distance, position2.y, position2.z);

        Vector3 camPos = cam3.transform.position;
        if (moveDoors)
        {
            moveCam = true;
            door1.transform.localPosition = Vector3.MoveTowards(position1, newDest1, movementSpeed);
            door2.transform.localPosition = Vector3.MoveTowards(position2, newDest2, movementSpeed);
        }
        if (moveCam)
        {
            cam3.transform.position = Vector3.MoveTowards(camPos, new Vector3(camPos.x + 0.1f, camPos.y, camPos.z), movementSpeed / 6);
        }

        if (position1.x < finalDest.x)
        {
            moveDoors = false;
        }
    }

    IEnumerator ManageTransitions()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
        cam3.SetActive(false);
        train.SetActive(false);
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.Change4);
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeIn());
        cam1.SetActive(false);
        cam2.SetActive(true);
        cam3.SetActive(false);
        train.SetActive(true);
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.Change7);
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeIn());
        cam1.SetActive(false);
        cam2.SetActive(false);
        cam3.SetActive(true);
        AudioManager.instance.InstantPlayFromAudioManager(soundsEnum.Change5);
        moveDoors = true;
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeIn());
        //transition to next scene
        LevelLoader.instance.LoadNextScene();
    }

    IEnumerator FadeIn()
    {
        while (image.alpha < 1)
        {
            image.alpha += fadeSpeed;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }

    IEnumerator FadeOut()
    {
        while (image.alpha > 0)
        {
            image.alpha -= 0.001f;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
