using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : Part
{
    public GameObject loadout;
    public float cooldownTime, energyCost;
    public bool isAuto;

    private float lastFireTime = -100;
    public float LastFireTime { get => lastFireTime; set => lastFireTime = value; }
}
