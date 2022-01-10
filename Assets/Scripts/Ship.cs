using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    Frame frame;
    [SerializeField]
    Shield shield;
    [SerializeField]
    Engine engine;
    [SerializeField]
    Generator generator;
    [SerializeField]
    Weapon primaryWeapon, secondaryWeapon;

    [SerializeField]
    Transform primarySpawn, secondarySpawn;

    Vector2 velocity;

    [SerializeField]
    SpriteRenderer engineSprite, shieldSprite;

    float engineVisability, shieldVisability;
    int credits;



    public float Fuel { get => frame.Currentfuel; }
    public float Hull { get => frame.CurrentHull; }
    public float CurrentShield { get => shield.CurrentShield; }
    public float Cargo { get => frame.CurrentCargo; }
    public Dictionary<ResourceType, int> Inventory { get => frame.Cargo; }
    public float Speed { get => velocity.magnitude; }
    public float Energy { get => generator.CurrentEnergy; }

    public float MaxFuel { get => frame.fuelCapacity; }
    public float MaxHull { get => frame.maxHull; }
    public float MaxShield { get => shield.maxShield; }
    public float MaxCargo { get => frame.cargoCapacity; }
    public float MaxEnergy { get => generator.maxEnergy; }

    public Vector2 Velocity { get => velocity; }
    public int Credits { get => credits; set => credits = value; }

    private void Start()
    {
        frame.Currentfuel = frame.fuelCapacity;
        frame.CurrentHull = frame.maxHull;
        MatchParts();
    }

    void MatchParts()
    {
        GetComponent<SpriteRenderer>().sprite = frame.sprite;
        shieldSprite.sprite = shield.sprite;
        engineSprite.sprite = engine.sprite;
    }

    public void Turn(float amount)
    {
        transform.Rotate(Vector3.back * amount * engine.turnThrust * Time.deltaTime);
    }

    public void Brake()
    {
        velocity = velocity.normalized * (velocity.magnitude - engine.brakeThrust * Time.deltaTime);
    }

    public void Accelerate(float amount)
    {
        if (frame.Currentfuel >= engine.fuelPerSecond * Time.deltaTime && generator.CurrentEnergy >= engine.energyCost * Time.deltaTime)
        {
            velocity += (Vector2)transform.up * amount * engine.mainThrust * Time.deltaTime;
            frame.Currentfuel -= engine.fuelPerSecond * Time.deltaTime;
            generator.CurrentEnergy -= engine.energyCost * Time.deltaTime;
            engineVisability = amount;
        }
    }

    public void Repair()
    {
        frame.Repair();
    }

    public void Refuel()
    {
        frame.Refuel();
    }

    public bool AddResource(ResourceType r)
    {
        return frame.AddResource(r);
    }

    public bool RemoveResource(ResourceType r)
    {
        return frame.RemoveResource(r);
    }

    private void FireWeapon(bool newPress, Weapon weapon, Transform spawn)
    {
        if (Time.time > weapon.LastFireTime + weapon.cooldownTime && weapon.energyCost <= generator.CurrentEnergy)
        {
            if (newPress || weapon.isAuto)
            {
                generator.CurrentEnergy -= weapon.energyCost;
                weapon.LastFireTime = Time.time;
                GameObject p = Instantiate(weapon.loadout, spawn.position, spawn.rotation);
                if (p.GetComponent<Missile>() != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 2, transform.up);
                    if (hit)
                    {
                        p.GetComponent<Missile>().SetTarget(hit.transform);
                    }
                }
            }
        }
    }

    public void FirePrimary(bool newPress)
    {
        FireWeapon(newPress, primaryWeapon, primarySpawn);
    }

    public void FireSecondary(bool newPress)
    {
        FireWeapon(newPress, secondaryWeapon, secondarySpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if(velocity.magnitude > engine.maxVelocity) { velocity = velocity.normalized * engine.maxVelocity; }
        transform.Translate(velocity * Time.deltaTime, Space.World);

        ShieldRegen();
        RunGenerator();
        Effects();
    }

    void ShieldRegen()
    {
        if(shield.CurrentShield < shield.maxShield && generator.CurrentEnergy >= shield.energyCost * Time.deltaTime)
        {
            shield.CurrentShield += shield.shieldRegenPerSecond * Time.deltaTime;
            generator.CurrentEnergy -= shield.energyCost * Time.deltaTime;
        }
        if(shield.CurrentShield > shield.maxShield)
        {
            shield.CurrentShield = shield.maxShield;
        }
    }

    void RunGenerator()
    {
        generator.CurrentEnergy += generator.energyPerSecond * Time.deltaTime;
        if(generator.CurrentEnergy > generator.maxEnergy) { generator.CurrentEnergy = generator.maxEnergy; }
    }

    void Effects() {
        if (engineVisability > 0)
        {
            engineSprite.color = new Color(1,1,1,engineVisability);
            engineVisability = 0;
        }
        else
        {
            engineSprite.color = new Color(1, 1, 1, 0);
        }

        if(shieldVisability > 0)
        {
            shieldSprite.color = new Color(1, 1, 1, shieldVisability);
            shieldVisability -= Time.deltaTime;
        }
        else
        {
            shieldSprite.color = new Color(1, 1, 1, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.collider.tag == "Asteroid")
        {
            float relativeVelocity = (collision.rigidbody.velocity - velocity).magnitude;
            if (relativeVelocity > .5f) { TakeDamage(relativeVelocity * 2, collision.transform.position); }

            collision.rigidbody.AddForce(velocity, ForceMode2D.Impulse);
            Vector2 direction = transform.position - collision.transform.position;
            velocity += direction.normalized * velocity.magnitude;
        }else if(collision.gameObject.tag == "Resource")
        {
            if (frame.AddResource(collision.gameObject.GetComponent<Resource>().Data))
            {
                Destroy(collision.gameObject);
            }
        }else if(collision.gameObject.tag == "Station")
        {
            collision.gameObject.GetComponent<Station>().OpenStation();
            transform.Translate((transform.position - collision.transform.position) * 2);
            velocity = Vector2.zero;
        }
    }

    public void TakeDamage(float amount, Vector2 source)
    {
        shieldSprite.transform.LookAt(source, Vector3.back);
        shieldSprite.transform.rotation = Quaternion.Euler(0, 0, shieldSprite.transform.rotation.eulerAngles.z);

        if(shield.CurrentShield > amount)
        {
            shield.CurrentShield -= amount;
            shieldVisability += 1;
        }
        else
        {
            amount -= shield.CurrentShield;
            shieldVisability = 1;
            shield.CurrentShield = 0;
            frame.CurrentHull -= amount;
        }
    }
}
