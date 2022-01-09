using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySellManager : MonoBehaviour
{
    public Button buy, sell;
    public Text buyText, sellText, label;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateUI(bool canBuy, bool canSell, int buyPrice, int sellPrice, string name)
    {
        label.text = name;
        if (canBuy)
        {
            buyText.text = "Buy - " + buyPrice + "C";
            buy.interactable = true;
        }
        else
        {
            buyText.text = "Cannot buy";
            buy.interactable = false;
        }
        if(canSell)
        {
            sellText.text = "Sell - " + sellPrice + "C";
            sell.interactable = true;
        }
        else
        {
            sellText.text = "Cannot sell";
            sell.interactable = false;
        }
    }
}
