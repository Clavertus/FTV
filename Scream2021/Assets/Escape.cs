using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed;
    [SerializeField] float speedBoost;
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    [SerializeField] GameObject monster;
    [SerializeField] GameObject playerCam; 

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
        player.transform.position = Vector3.MoveTowards(player.transform.position, pointB.transform.position, moveSpeed * Time.deltaTime);
    }
    private void MoveToPointC()
    {
        if(player.transform.position == pointC.transform.position) { return; }
        FindObjectOfType<MouseLook>().UnlockCamera();
        monster.GetComponent<MonsterAction>().MonsterInTheDoor(); 
        player.transform.position = Vector3.MoveTowards(player.transform.position, pointC.transform.position, speedBoost * Time.deltaTime);
    }
    void MoveToPointD()
    {
        AudioManager.instance.InstantStopFromAudioManager(soundsEnum.Drone);
        
        AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<OpenSideDoor>().myAudioSource);
        AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterAgressive);
        AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterAgressive2);
        AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterAttack);
        AudioManager.instance.InstantStopFromGameObject(FindObjectOfType<MonsterAction>().monsterBreathe);
        







        if (player.transform.position == pointD.transform.position) { return; }
        LevelLoader.instance.ending = Ending.Good;
        StartCoroutine(LevelLoader.instance.StartLoadingNextScene());   
        player.transform.position = Vector3.MoveTowards(player.transform.position, pointD.transform.position, moveSpeed * Time.deltaTime);
    }
    
}
