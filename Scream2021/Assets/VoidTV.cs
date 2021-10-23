using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidTV : MonoBehaviour
{
    public Material TVstatic;

      public  void Update()
    {

        TVstatic.mainTextureOffset += new Vector2(Time.deltaTime * 3, 0);
    }
}
