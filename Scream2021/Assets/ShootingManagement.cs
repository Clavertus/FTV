using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShootingManagement : MonoBehaviour
{

    public MeshRenderer pistol;

    public ParticleSystem shot;
    public Animator anim;

    public float fireCD = 0;
    public float relCD = 0;

    public int currAmmo;
    public int totalAmmo;

    public int clipCapacity = 6;

    public Text text_ca; //currAmmo
    public Text text_ta; //totalAmmo
    public GameObject reloadBar;
    public GameObject reloadBarMax;

    public bool reloading = false;




    void Start()
    {

        currAmmo = clipCapacity;
        totalAmmo = 30;

        text_ca = GameObject.Find("currentAmmo").GetComponent<Text>();
        text_ta = GameObject.Find("totalAmmo").GetComponent<Text>();
        reloadBar = GameObject.Find("ReloadVal");
        reloadBarMax = GameObject.Find("ReloadValMax");
        UpdateAmmoInfo();

    }

    void Update()
    {
        reloadBar.GetComponent<RectTransform>().sizeDelta = new Vector2(fireCD * 100, reloadBar.GetComponent<RectTransform>().sizeDelta.y);
        reloadBarMax.GetComponent<RectTransform>().sizeDelta = new Vector2(relCD * 33.33f, reloadBarMax.GetComponent<RectTransform>().sizeDelta.y);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pistol.enabled = true;

        if(fireCD > 0)
        {
            fireCD -= Time.deltaTime;
        }

        if (relCD > 0 && reloading)
        {
            relCD -= Time.deltaTime;
        } else
        {
            if(reloading == true)
            {
                reloading = false;
                totalAmmo -= clipCapacity;
                currAmmo = clipCapacity;
                UpdateAmmoInfo();
            }
            
            
        }


        if (Input.GetButton("Fire1") && fireCD <= 0f && currAmmo > 0 && !reloading)
        {
            Shoot();        
        }

        if (Input.GetButton("Fire1") && fireCD <= 0f && currAmmo == 0 && !reloading)
        {
            Reload();
        }

        if(Input.GetButton("Reload") && !reloading && currAmmo != clipCapacity)
        {
            Reload();
        }

    }

    void Reload()
    {
        relCD = 3f;
        reloading = true;
    }

    void UpdateAmmoInfo()
    {
        text_ca.text = currAmmo.ToString();
        text_ta.text = totalAmmo.ToString();
    }

    void Shoot()
    {

        fireCD = 1f;
        currAmmo -= 1;
        UpdateAmmoInfo();

        shot.Play();
        anim.Play("pistol_shot");


        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
        }

        if(currAmmo == 0)
        {
            Reload();
        }

    }


}
