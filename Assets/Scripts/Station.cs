using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour
{
    private int repairPrice, fuelPrice;
    private Dictionary<ResourceType, int> buyPrices = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, int> sellPrices = new Dictionary<ResourceType, int>();
    private Dictionary<ResourceType, BuySellManager> menuLines = new Dictionary<ResourceType, BuySellManager>();
    [SerializeField]
    private GameObject stationMenu, buySellMenu, menuLinePrefab, repair, fuel;


    Player player;

    // Start is called before the first frame update
    void Start()
    {
        repairPrice = Random.Range(1, 5);
        fuelPrice = Random.Range(1, 3);

        foreach(ResourceType r in ResourceLedger.RTs)
        {
            int price = Random.Range(5, 40);
            buyPrices.Add(r, price);
            price = Mathf.RoundToInt(price * Random.Range(.5f, 1.1f));
            sellPrices.Add(r, price);
            menuLines.Add(r, Instantiate(menuLinePrefab, buySellMenu.transform).GetComponent<BuySellManager>());

            menuLines[r].SetVars(this, r);
        }


        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        player = FindObjectOfType<Player>();
    }

    public void OpenStation()
    {
        stationMenu.SetActive(true);
        player.gameObject.SetActive(false);
        SetButtons();
    }

    public void CloseStation()
    {
        stationMenu.SetActive(false);
        player.gameObject.SetActive(true);
    }

    void SetButtons()
    {
        foreach(ResourceType r in ResourceLedger.RTs)
        {
            menuLines[r].UpdateUI(
                player.Self.Cargo + r.weight <= player.Self.MaxCargo && player.Self.Credits - buyPrices[r] >= 0, //Can Buy if player has enough cargo and credits
                player.Self.Inventory.ContainsKey(r) && player.Self.Inventory[r] >= 1, //Can Sell if player has at least one of the resource
                buyPrices[r],
                sellPrices[r],
                r.name
                );
        }

        //Repair
        if (player.Self.Hull < player.Self.MaxHull)
        {
            if (player.Self.Credits > repairPrice)
            {
                repair.GetComponent<Button>().interactable = true;
                repair.GetComponentInChildren<Text>().text = "Repair - " + repairPrice + "C";
            }
            else
            {
                repair.GetComponent<Button>().interactable = false;
                repair.GetComponentInChildren<Text>().text = "Can't afford - " + repairPrice + "C";
            }
        }
        else
        {
            repair.GetComponent<Button>().interactable = false;
            repair.GetComponentInChildren<Text>().text = "Fully repaired";
        }

        //Fuel
        if (player.Self.Fuel < player.Self.MaxFuel)
        {
            if (player.Self.Credits > fuelPrice)
            {
                fuel.GetComponent<Button>().interactable = true;
                fuel.GetComponentInChildren<Text>().text = "Refuel - " + fuelPrice + "C";
            }
            else
            {
                fuel.GetComponent<Button>().interactable = false;
                fuel.GetComponentInChildren<Text>().text = "Can't afford - " + fuelPrice + "C";
            }
        }
        else
        {
            fuel.GetComponent<Button>().interactable = false;
            fuel.GetComponentInChildren<Text>().text = "Fully fueled";
        }
    }

    public void Repair()
    {
        player.Self.Repair();
        player.Self.Credits -= repairPrice;
        SetButtons();
    }

    public void BuyFuel()
    {
        player.Self.Refuel();
        player.Self.Credits -= fuelPrice;
        SetButtons();
    }

    public void Buy(ResourceType r)
    {
        if(player.Self.Credits >= buyPrices[r] && player.Self.Cargo + r.weight <= player.Self.MaxCargo)
        {
            if (player.Self.AddResource(r))
            {
                player.Self.Credits -= buyPrices[r];
            }
            SetButtons();
        }
    }

    public void Sell(ResourceType r)
    {
        if (player.Self.Inventory.ContainsKey(r) && player.Self.Inventory[r] >= 1)
        {
            if (player.Self.RemoveResource(r))
            {
                player.Self.Credits += sellPrices[r];
            }
            SetButtons();
        }
    }
}
