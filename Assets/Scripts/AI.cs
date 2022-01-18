using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI : MonoBehaviour
{
    public float combatDistance, miningDistance;

    Ship self;

    public Ship Self { get => self; }
    public Intents Intent { get => intent; }

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Ship>();
    }

    GameObject target;
    Intents intent;

    public void React(Intents i, GameObject t)
    {
        intent = i;
        target = t;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (intent)
        {
            case Intents.None:
                NewIntent();
                break;
            case Intents.Station:
                Station();
                break;
            case Intents.Mine:
                Mine();
                break;
            case Intents.Collect:
                Collect();
                break;
            case Intents.Fight:
                Fight();
                break;
            case Intents.Flee:
                RunAway();
                break;
        }
    }
    void NewIntent()
    {
        List<Collider2D> hits = new List<Collider2D>();
        hits = Physics2D.OverlapCircleAll(transform.position, 50).OrderBy(
            x => Vector2.Distance(this.transform.position, x.transform.position)
                ).ToList();

        GameObject station = null;
        GameObject ship = null;
        GameObject asteroid = null;
        GameObject resource = null;

        foreach (Collider2D c in hits)
        {
            if(station != null && ship != null && asteroid != null && resource != null) { break; }

            if (c.tag == "Asteroid")
            {
                if(asteroid == null) { asteroid = c.gameObject; }
            }
            else if (c.tag == "Ship" && c.gameObject != gameObject)
            {
                if (ship == null) { ship = c.gameObject; }
            }
            else if (c.tag == "Station")
            {
                if (station == null) { station = c.gameObject; }
            }
            else if(c.tag == "Resource")
            {
                if (resource == null) { resource = c.gameObject; }
            }
        }

        if(self.Fuel < self.MaxFuel/5 || self.Cargo > self.MaxCargo*4/5 || self.Hull < self.MaxHull / 2)
        {
            if(station != null)
            {
                intent = Intents.Station;
                target = station;
            }
            else
            {
                intent = Intents.Flee;
            }
            
        }else if(resource != null && resource.GetComponent<Resource>().Data.weight + self.Cargo <= self.MaxCargo){
            intent = Intents.Collect;
            target = resource;
        }else if(asteroid != null)
        {
            intent = Intents.Mine;
            target = asteroid;
        }else if(ship != null)
        {
            intent = Random.Range(1, 4) > 2 ? Intents.Fight : Intents.Flee;
            target = ship;
        }
    }

    void Mine()
    {
        if (target == null)
        {
            NewIntent();
            return;
        }
        Vector2 goal = target.transform.position + (transform.position - target.transform.position).normalized * miningDistance;

        if (Vector2.Distance(transform.position, target.transform.position) <= miningDistance + .5f)
        {
            Aim(target.transform.position - transform.position);
            if (Vector2.Dot(transform.up.normalized, target.transform.position - transform.position) > 0f)
            {
                self.FirePrimary(false);
            }
        }
        else
        {
            FlyTo(goal);
        }
    }

    void Station()
    {
        if(target == null)
        {
            NewIntent();
            return;
        }

        FlyTo(target.transform.position);
    }

    void RunAway()
    {
        if(target == null)
        {
            FlyTo(transform.up * 10);
            NewIntent();
            return;
        }

        FlyTo(transform.position - target.transform.position);
    }

    void Collect()
    {
        if (target == null)
        {
            NewIntent();
            return;
        }

        FlyTo(target.transform.position);
    }

    void FlyTo(Vector2 location)
    {
        Vector2 desiredVelocity = (location - (Vector2)transform.position).normalized * (Vector2.Distance(transform.position, location) * self.Engine.BrakeThrust);
        Vector2 desiredDirection = desiredVelocity - self.Velocity;

        Aim(desiredVelocity);
        float aimAlignment = Vector2.Dot(transform.up.normalized, desiredDirection.normalized);
        float velocityAlignment = Vector2.Dot(desiredVelocity, self.Velocity);

        if (aimAlignment > 0f)
        {
            if (self.Speed == 0 || (velocityAlignment > 0 && self.Velocity.magnitude <= desiredVelocity.magnitude))
            {
                self.Accelerate(aimAlignment);
            }
            else
            {
                self.Brake();
            }

        }
        else if (aimAlignment < 0 && (velocityAlignment < 0 || self.Velocity.magnitude > desiredVelocity.magnitude))
        {
            self.Brake();
        }
    }

    void Aim(Vector2 direction)
    {
        switch (Helpers.RightOrLeft(transform, transform.position + (Vector3)direction))
        {
            case 1:
                self.Turn(-1);
                break;
            case 2:
                self.Turn(1);
                break;
        }
    }

    public void Trade(Station station)
    {
        station.TransactionWithAI(Self);
        NewIntent();
    }

    void Fight()
    {
        if (target == null) {
            NewIntent();
            return;
        }
        Vector2 goal = target.transform.position + (transform.position - target.transform.position).normalized * combatDistance;

        if(Vector2.Distance(transform.position, target.transform.position) <= combatDistance + .5f)
        {
            Aim(target.transform.position - transform.position);
            if (Vector2.Dot(transform.up.normalized, target.transform.position - transform.position) > 0f)
            {
                self.FireSecondary(Random.Range(0, 4) > 2);
            }
        }
        else
        {
            FlyTo(goal);
        }

    }

    public enum Intents
    {
        None,
        Station,
        Mine,
        Collect,
        Fight,
        Flee
    }
}
