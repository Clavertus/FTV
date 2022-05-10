using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidTV : MonoBehaviour
{
    public Material TVstatic;

    public Color stageZero;
    public Color stageOne;
    public Color stageTwo;
    public Color stageThree;

    public int materialState;

      public  void Update()
    {
        TVstatic.mainTextureOffset += new Vector2(Time.deltaTime * 3, 0);

        switch (materialState)
        {
            case 0:
                TVstatic.color = stageZero; break;

            case 1:
                TVstatic.color = stageOne; break;

            case 2:
                TVstatic.color = stageTwo; break;

            case 3:
                TVstatic.color = stageThree; break;

            default:
                TVstatic.color = stageZero; break;

        }
    }


}
