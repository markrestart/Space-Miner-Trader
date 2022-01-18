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

    public void Repair(int amount = 1)
    {
        currentHull+= amount;
        if(currentHull > maxHull) { currentHull = maxHull; }
    }

    public void Refuel(int amount = 1)
    {
        currentfuel+= amount;
        if(currentfuel > fuelCapacity) { currentfuel = fuelCapacity; }
    }

    public void ClearInventory()
    {
        cargo.Clear();
        cargoWeight = 0;
    }

    public float CurrentHull { get => currentHull; set => currentHull = value; }
    public float CurrentCargo { get => cargoWeight; }
    public float Currentfuel { get => currentfuel; set => currentfuel = value; }
    public Dictionary<ResourceType, int> Cargo { get => cargo; }
}
