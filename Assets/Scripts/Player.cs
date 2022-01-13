using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Ship self;

    public Ship Self { get => self; }

    Minimap map;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Ship>();
        map = GetComponentInChildren<Minimap>();
        ResourceLedger.Initialize();
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

        List<MapObject> objects = new List<MapObject>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 50);
        foreach(Collider2D c in hits)
        {
            if(c.tag == "Asteroid")
            {
                objects.Add(new MapObject(c.transform.position - transform.position, 1));
            }else if(c.tag == "Ship" && c.gameObject != gameObject)
            {
                objects.Add(new MapObject(c.transform.position - transform.position, 2));
            }
            else if(c.tag == "Station")
            {
                objects.Add(new MapObject(c.transform.position - transform.position, 3));
            }
        }

        map.UpdateMap(objects);
    }
}
