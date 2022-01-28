using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig", order = 0)]
public class WeaponConfig : ScriptableObject
{
    public float maxAmmo;
    public float damage;
    public bool areaDamage;
}
