using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EndlessTrainLightSetup : MonoBehaviour
{
    [SerializeField] Color lightEmitColor = Color.white;
    [SerializeField] float lightEmitPower = 1f;
    [SerializeField] float innerAngle = 10f;
    [SerializeField] float outerAngle = 100f;
    [SerializeField] float lightRange = 10f;
    Light[] lights;

    private void Start()
    {
        lights = GetComponentsInChildren<Light>();
        SetupLight();
    }

    private void Update()
    {
        //remove this in game!
        SetupLight();
    }

    private void SetupLight()
    {
        foreach (Light light in lights)
        {
            light.color = lightEmitColor;
            light.intensity = lightEmitPower;
            light.spotAngle = outerAngle;
            float innerAngleByLight = innerAngle;
            if (innerAngle > outerAngle)
            {
                innerAngleByLight = outerAngle;
            }
            light.innerSpotAngle = innerAngleByLight;
            light.range = lightRange;
        }
    }
}
