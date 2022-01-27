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
    private GameObject stationMenu, buySellMenu, menuLinePrefab, repair, fuel, maxRepair, maxFuel;
    [SerializeField]
    private Text nameLabel;

    private float mechNeeds, bioNeeds, techNeeds;


    Player player;

    public float BioNeeds { get => bioNeeds; set { bioNeeds = value; if (bioNeeds < 0) { bioNeeds = 0; } if (bioNeeds > 100) { bioNeeds = 100; } } }
    public float TechNeeds { get => techNeeds; set { techNeeds = value; if (techNeeds < 0) { techNeeds = 0; } if (techNeeds > 100) { techNeeds = 100; } } }
    public float MechNeeds { get => mechNeeds; set { mechNeeds = value; if (mechNeeds < 0) { mechNeeds = 0; } if (mechNeeds > 100) { mechNeeds = 100; } } }

    static string[] prefix = new string[] { "Gold ", "Yellow ", "Violet ", "Mining ", "Deep Space ", "Eagle ", "Viper ", "Titan ", "Deeprock ", "Star ", "Farpoint ", "Babylon ", "Elysium ", "Voyager ", "Apollo ", "Bradbury ", "Galaxy ", "Nova " };
    static string[] middle = new string[] { "Base ", "Station ", "Outpost ", "Trader ", "City ", "Platform ", "Refuge ", "Terminal ", "Research ", "Observation ", "Relay ", "Hub " };
    static string[] suffix = new string[] { "Alpha", "Beta", "Delta", "Gamma ", "Epsilon", "Zeta", "Pi", "Sigma", "Theta", "Omicron", "Omega", "Prime", "Second", "Nine", "Five", "Seven", "One" };

    // Start is called before the first frame update
    void Start()
    {
        BioNeeds = Random.Range(30, 70);
        MechNeeds = Random.Range(30, 70);
        TechNeeds = Random.Range(30, 70);

        repairPrice = Random.Range(1, 5);
        fuelPrice = Random.Range(1, 3);

        string name = prefix[Random.Range(0, prefix.Length)] + middle[Random.Range(0, middle.Length)] + suffix[Random.Range(0, suffix.Length)];
        nameLabel.text = name;

        foreach (ResourceType r in ResourceLedger.RTs)
        {
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

    void SetPrices()
    {
        repairPrice = Mathf.FloorToInt( 1 + mechNeeds/20);
        fuelPrice = Mathf.FloorToInt(1 + bioNeeds / 50);

        foreach (ResourceType r in ResourceLedger.RTs)
        {
            int price = Mathf.FloorToInt((r.bioValue * bioNeeds + r.mechValue * mechNeeds + r.techValue * techNeeds) / 100 + r.intrinsicValue);

            buyPrices[r] = price;

            sellPrices[r] = Mathf.FloorToInt(price * (1 + techNeeds/100));

        }
    }

    public void TransactionWithAI(Ship ai)
    {
        SetPrices();

        foreach (KeyValuePair<ResourceType, int> r in ai.Inventory)
        {
            {
                ai.Credits += sellPrices[r.Key] * r.Value;

                BioNeeds -= (r.Key.bioValue / 10f) * r.Value;
                TechNeeds -= (r.Key.techValue / 10f) * r.Value;
                MechNeeds -= (r.Key.mechValue / 10f) * r.Value;
            }
        }

        int fuel = Mathf.Clamp(ai.Credits / fuelPrice, 0, Mathf.CeilToInt(ai.MaxFuel-ai.Fuel));
        ai.Refuel(fuel);
        ai.Credits -= fuel * fuelPrice;

        int hull = Mathf.Clamp(ai.Credits / repairPrice, 0, Mathf.CeilToInt(ai.MaxHull - ai.Hull));
        ai.Repair(hull);
        ai.Credits -= hull * repairPrice;

        ai.ClearInventory();

        SetButtons();
    }

    public void SetButtons()
    {
        SetPrices();
        foreach(ResourceType r in ResourceLedger.RTs)
        {
            menuLines[r].UpdateUI(
                player.Self.Cargo + r.weight * menuLines[r].Amount <= player.Self.MaxCargo && player.Self.Credits - buyPrices[r] * menuLines[r].Amount >= 0, //Can Buy if player has enough cargo and credits
                player.Self.Inventory.ContainsKey(r) && player.Self.Inventory[r] >= menuLines[r].Amount, //Can Sell if player has at least one of the resource
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

                int maxRepairAmount = (int)Mathf.Clamp(player.Self.Credits / repairPrice, 0, player.Self.MaxHull - player.Self.Hull + 1);
                maxRepair.GetComponent<Button>().interactable = true;
                maxRepair.GetComponentInChildren<Text>().text = "Repair " + maxRepairAmount + " - " + repairPrice * maxRepairAmount + "C";
            }
            else
            {
                repair.GetComponent<Button>().interactable = false;
                repair.GetComponentInChildren<Text>().text = "Can't afford - " + repairPrice + "C";

                maxRepair.GetComponent<Button>().interactable = false;
                maxRepair.GetComponentInChildren<Text>().text = "Can't afford - " + repairPrice + "C";
            }
        }
        else
        {
            repair.GetComponent<Button>().interactable = false;
            repair.GetComponentInChildren<Text>().text = "Fully repaired";

            maxRepair.GetComponent<Button>().interactable = false;
            maxRepair.GetComponentInChildren<Text>().text = "Fully repaired";
        }

        //Fuel
        if (player.Self.Fuel < player.Self.MaxFuel)
        {
            if (player.Self.Credits > fuelPrice)
            {
                fuel.GetComponent<Button>().interactable = true;
                fuel.GetComponentInChildren<Text>().text = "Refuel - " + fuelPrice + "C";

                int maxFuelAmount = (int)Mathf.Clamp(player.Self.Credits / fuelPrice, 0, player.Self.MaxFuel - player.Self.Fuel + 1);
                maxFuel.GetComponent<Button>().interactable = true;
                maxFuel.GetComponentInChildren<Text>().text = "Refuel " + maxFuelAmount + " - " + fuelPrice * maxFuelAmount + "C";
            }
            else
            {
                fuel.GetComponent<Button>().interactable = false;
                fuel.GetComponentInChildren<Text>().text = "Can't afford - " + fuelPrice + "C";

                maxFuel.GetComponent<Button>().interactable = false;
                maxFuel.GetComponentInChildren<Text>().text = "Can't afford - " + fuelPrice + "C";
            }
        }
        else
        {
            fuel.GetComponent<Button>().interactable = false;
            fuel.GetComponentInChildren<Text>().text = "Fully fueled";

            maxFuel.GetComponent<Button>().interactable = false;
            maxFuel.GetComponentInChildren<Text>().text = "Fully fueled";
        }
    }

    public void Repair()
    {
        player.Self.Repair();
        player.Self.Credits -= repairPrice;
        MechNeeds++;
        SetButtons();
    }

    public void BuyFuel()
    {
        player.Self.Refuel();
        player.Self.Credits -= fuelPrice;
        BioNeeds++;
        SetButtons();
    }

    public void RepairMax()
    {
        int maxRepairAmount = (int)Mathf.Clamp(player.Self.Credits/repairPrice, 0, player.Self.MaxHull - player.Self.Hull + 1);
        player.Self.Repair(maxRepairAmount);
        player.Self.Credits -= repairPrice;
        MechNeeds+= maxRepairAmount;
        SetButtons();
    }

    public void BuyFuelMax()
    {
        int maxFuelAmount = (int)Mathf.Clamp(player.Self.Credits/fuelPrice, 0, player.Self.MaxFuel - player.Self.Fuel + 1);
        player.Self.Refuel(maxFuelAmount);
        player.Self.Credits -= fuelPrice * maxFuelAmount;
        BioNeeds+= maxFuelAmount;
        SetButtons();
    }

    public void Buy(ResourceType r, int amount = 1)
    {
        if(player.Self.Credits >= buyPrices[r] * amount && player.Self.Cargo + r.weight * amount <= player.Self.MaxCargo)
        {
            if (player.Self.AddResource(r))
            {
                player.Self.Credits -= buyPrices[r] * amount;

                BioNeeds += r.bioValue * amount / 10f;
                TechNeeds += r.techValue * amount / 10f;
                MechNeeds += r.mechValue * amount / 10f;
            }
            SetButtons();
        }
    }

    public void Sell(ResourceType r, int amount = 1)
    {
        if (player.Self.Inventory.ContainsKey(r) && player.Self.Inventory[r] >= amount)
        {
            if (player.Self.RemoveResource(r, amount))
            {
                player.Self.Credits += sellPrices[r] * amount;

                BioNeeds -= r.bioValue * amount / 10f;
                TechNeeds -= r.techValue * amount / 10f;
                MechNeeds -= r.mechValue * amount / 10f;
            }
            SetButtons();
        }
    }
}
