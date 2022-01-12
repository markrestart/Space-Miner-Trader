using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Engine : Part
{
    [SerializeField]
    private float turnThrust, mainThrust, brakeThrust, maxVelocity, fuelPerSecond, energyCost;
    [SerializeField]
    private Sprite sprite;

    public float TurnThrust { get => turnThrust; }
    public float MainThrust { get => mainThrust; }
    public float BrakeThrust { get => brakeThrust; }
    public float MaxVelocity { get => maxVelocity; }
    public float FuelPerSecond { get => fuelPerSecond; }
    public float EnergyCost { get => energyCost; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}
