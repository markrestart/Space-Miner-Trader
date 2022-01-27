using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySellManager : MonoBehaviour
{
    public Button buy, sell;
    public Text buyText, sellText, label;
    private Station station;
    private ResourceType resource;

    private int amount = 1;

    public int Amount { get => amount; }

    // Start is called before the first frame update
    void Start()
    {
 
    }

    public void SetVars(Station s, ResourceType r)
    {
        station = s;
        resource = r;
    }

    // Update is called once per frame
    public void UpdateUI(bool canBuy, bool canSell, int buyPrice, int sellPrice, string name)
    {
        label.text = name;
        if (canBuy)
        {
            buyText.text = "Buy "+ amount + " - " + buyPrice * amount + "C";
            buy.interactable = true;
        }
        else
        {
            buyText.text = "Cannot buy";
            buy.interactable = false;
        }
        if(canSell)
        {
            sellText.text = "Sell " + amount + " - " + sellPrice * amount + "C";
            sell.interactable = true;
        }
        else
        {
            sellText.text = "Cannot sell";
            sell.interactable = false;
        }

        if(!canBuy && !canSell && amount >= 1)
        {
            amount--;
            station.SetButtons();
        }
    }

    public void More()
    {
        amount++;
        station.SetButtons();
    }

    public void Less()
    {
        if(amount > 1)
        {
            amount--;
            station.SetButtons();
        }
    }

    public void Buy()
    {
        station.Buy(resource, amount);
    }

    public void Sell()
    {
        station.Sell(resource, amount);
    }
}
