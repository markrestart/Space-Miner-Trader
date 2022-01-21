using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shield : Part
{
    //TODO: Make these private, serializefield, make getters
    public float maxShield, shieldRegenPerSecond, energyCost;
    public Sprite sprite;
    private float currentShield;

    [SerializeField]
    private AudioClip sound;
    public float CurrentShield { get => currentShield; set => currentShield = value; }
    public AudioClip Sound { get => sound; }
}
