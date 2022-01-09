using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Engine : Part
{
    public float turnThrust, mainThrust, brakeThrust, maxVelocity, fuelPerSecond, energyCost;
    public Sprite sprite;
}
