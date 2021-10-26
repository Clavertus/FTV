using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float moveSpeedB;
    [SerializeField] float moveSpeedC;
    [SerializeField] float moveSpeedD;
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject monster;

    [SerializeField] Transform pointB;
    [SerializeField] Transform pointC;
    [SerializeField] Transform pointD;

    bool moveToPointB = false; 
    bool hitPointB = false;
    bool hitPointC = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(tag == "Selected") { StartEscape(); }
        if (moveToPointB && !hitPointB) { MoveToPointB(); }
        if (player.transform.position == pointB.transform.position)
        {
            Debug.Log("hit");
            hitPointB = true;
        }
        if (hitPointB)
        {
            MoveToPointC();
        }

        if(player.transform.position == pointC.transform.position)
        {
            hitPointC = true;
               
            door1.GetComponent<AutoOpen>().PushDoor();
            door2.GetComponent<AutoOpen>().PushDoor();

        }
        if (hitPointC)
        {
            MoveToPointD();
        }
    }
    private void StartEscape()
    {
        monster.GetComponent<MonsterAction>().StopMoving(); 
        player.GetComponentInChildren<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        FindObjectOfType<PlayerMovement>().LockPlayer();
        moveToPointB = true;  
    }
    void MoveToPointB()
    {
        if(player.transform.position == pointB.transform.position) { return; }
        player.transform.rotation = Quaternion.Euler(0, 90, 0);  
        FindObjectOfType<MouseLook>().LockCamera();
        player.transform.position = Vector3.MoveTowards(player.transform.position, pointB.transform.position, moveSpeedB * Time.deltaTime);
    }
    private void MoveToPointC()
    {
        if(player.transform.position == pointC.transform.position) { return; }
        //FindObjectOfType<MouseLook>().UnlockCamera();
        monster.GetComponent<MonsterAction>().MonsterInTheDoor(); 
        player.transform.position = Vector3.MoveTowards(player.transform.position, pointC.transform.position, moveSpeedC * Time.deltaTime);
    }
    void MoveToPointD()
    {
        if (player.transform.position == pointD.transform.position) { return; }
        if (FindObjectOfType<MonsterAction>())
        {
            AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterAgressive);
            AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterAgressive2);
            AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterAttack);
            AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterBreathe);
        }

        AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<OpenSideDoor>().myAudioSource);
        LevelLoader.instance.ending = Ending.Good;
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());   
        player.transform.position = Vector3.MoveTowards(player.transform.position, pointD.transform.position, moveSpeedD * Time.deltaTime);
    }
    
}
