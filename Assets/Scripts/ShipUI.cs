using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    [SerializeField]
    Text SpeedText, FuelText, HullText, ShieldText, CargoText, EnergyText, CreditsText;

    [SerializeField]
    Slider FuelBar, HullBar, ShieldBar, CargoBar, EnergyBar;

    Ship self;


    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedText.text = "Speed: " + self.Speed.ToString("0.#");
        CreditsText.text = "Credits: " + self.Credits;
        FuelText.text = "Fuel: " + self.Fuel.ToString("0.#");
        HullText.text = "Hull: " + self.Hull.ToString("0.#");
        ShieldText.text = "Shield: " + self.CurrentShield.ToString("0.#");
        CargoText.text = "Cargo: " + self.Cargo.ToString("0.#");
        EnergyText.text = "Energy: " + self.Energy.ToString("0.#");

        FuelBar.value = self.Fuel / self.MaxFuel;
        HullBar.value = self.Hull / self.MaxHull;
        ShieldBar.value = self.CurrentShield / self.MaxShield;
        CargoBar.value = self.Cargo / self.MaxCargo;
        EnergyBar.value = self.Energy / self.MaxEnergy;
    }
}
