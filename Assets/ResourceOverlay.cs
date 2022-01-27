using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceOverlay : MonoBehaviour
{
    [SerializeField]
    Text label;
    [SerializeField]
    Slider bio, tech, mech, intrinsic;

    public void Initialize(ResourceType data)
    {
        label.text = data.name;
        bio.value = (float)data.bioValue / 10;
        tech.value = (float)data.techValue / 10;
        mech.value = (float)data.mechValue / 10;
        intrinsic.value = (float)data.intrinsicValue / 10;
    }
}
