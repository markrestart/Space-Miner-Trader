using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shield : Part
{
    public float maxShield, shieldRegenPerSecond, energyCost;
    public Sprite sprite;
    private float currentShield;
    public float CurrentShield { get => currentShield; set => currentShield = value; }
}
