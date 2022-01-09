using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Frame : Part
{
    public Sprite sprite;
    public float maxHull, cargoCapacity, fuelCapacity;
    private float currentHull, cargoWeight, currentfuel;
    private Dictionary<ResourceType, int> cargo = new Dictionary<ResourceType, int>();

    public bool AddResource(ResourceType data)
    {
        if(cargoWeight + data.weight <= cargoCapacity)
        {
            cargoWeight += data.weight;
            if (cargo.ContainsKey(data))
            {
                cargo[data]++;
            }
            else
            {
                cargo.Add(data, 1);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RemoveResource(ResourceType data)
    {
        if (cargo.ContainsKey(data) && cargo[data] >= 1)
        {
            cargo[data]--;
            cargoWeight -= data.weight;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Repair()
    {
        currentHull++;
        if(currentHull > maxHull) { currentHull = maxHull; }
    }

    public float CurrentHull { get => currentHull; set => currentHull = value; }
    public float CurrentCargo { get => cargoWeight; }
    public float Currentfuel { get => currentfuel; set => currentfuel = value; }
    public Dictionary<ResourceType, int> Cargo { get => cargo; }
}
