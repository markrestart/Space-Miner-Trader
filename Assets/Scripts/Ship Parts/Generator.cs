using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Generator : Part
{
    public float energyPerSecond, maxEnergy;
    private float currentEnergy;
    public float CurrentEnergy { get => currentEnergy; set => currentEnergy = value; }
}
