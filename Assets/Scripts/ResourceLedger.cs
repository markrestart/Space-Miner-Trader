using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLedger 
{
    public static void Initialize()
    {
        RTs = new List<ResourceType>();
        RTs.Add(new ResourceType(3, "Enginium"));
        RTs.Add(new ResourceType(1, "Hyperchem"));
        RTs.Add(new ResourceType(1, "Gigametal"));
    }

    public static List<ResourceType> RTs;
}

public struct ResourceType
{
    public float weight;
    public string name;

    public ResourceType(float weight, string name)
    {
        this.weight = weight;
        this.name = name;
    }
}
