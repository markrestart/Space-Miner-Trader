using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Ship self;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0) { self.Turn(Input.GetAxis("Horizontal")); }

        if (Input.GetAxis("Vertical") > 0) { self.Accelerate(Input.GetAxis("Vertical")); }

        if (Input.GetAxis("Vertical") < 0) { self.Brake(); }

        if(Input.GetButtonDown("Fire1")) { self.FirePrimary(true); } else
        if (Input.GetButton("Fire1")) { self.FirePrimary(false); }

        if (Input.GetButtonDown("Fire2")) { self.FireSecondary(true); } else
        if (Input.GetButton("Fire2")) { self.FireSecondary(false); }
    }
}
