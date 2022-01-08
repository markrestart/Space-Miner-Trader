using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    [SerializeField]
    Text UI;

    Ship self;


    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        UI.text = "Fuel: " + self.Fuel + "\nHull: " + self.Hull + "\nShield: " + self.Shield + "\nCargo: " + self.Cargo + "\nSpeed: " + self.Speed;
    }
}
