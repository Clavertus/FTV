using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuCotrols : MonoBehaviour
{
    bool menuActive = false;

    [SerializeField] GameObject[] childrens = null;
    // Start is called before the first frame update
    void Start()
    {
        menuActive = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ChildrenHandle(menuActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuActive)
        {
            menuActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ChildrenHandle(menuActive);
            Time.timeScale = 0;
            return;
        }

        if (menuActive)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                menuActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                ChildrenHandle(menuActive);
                Time.timeScale = 1;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Application.Quit();
                return;
            }
        }
    }

    private void ChildrenHandle(bool enable)
    {
        foreach (var children in childrens)
        {
            children.gameObject.SetActive(enable);
        }
    }
}
