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
    AudioSource engineAudio, shieldAudio;

    [SerializeField]
    LayerMask targetable;

    [SerializeField]
    Transform weaponSpawn;

    Vector2 velocity;

    [SerializeField]
    SpriteRenderer engineSprite, shieldSprite;

    float engineVisability, shieldVisability;
    int credits;

    [SerializeField]
    GameObject explosion, resourceDrop;

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
    public Engine Engine { get => engine;}

    private void Start()
    {
        frame.Currentfuel = frame.fuelCapacity;
        frame.CurrentHull = frame.maxHull;
        shield.CurrentShield = shield.maxShield;
        generator.CurrentEnergy = generator.maxEnergy;
        MatchParts();
    }

    void MatchParts()
    {
        GetComponent<SpriteRenderer>().sprite = frame.sprite;
        shieldSprite.sprite = shield.sprite;
        engineSprite.sprite = engine.Sprite;
        engineAudio.clip = engine.Sound;
        engineAudio.volume = 0;
        engineAudio.Play();
        shieldAudio.clip = shield.Sound;
    }

    public void Turn(float amount)
    {
        transform.Rotate(Vector3.back * amount * engine.TurnThrust * Time.deltaTime);
    }

    public void Brake()
    {
        velocity = velocity.normalized * (velocity.magnitude - engine.BrakeThrust * Time.deltaTime);
    }

    public void Accelerate(float amount)
    {
        amount = Mathf.Clamp(amount, .3f, 1);
        if (frame.Currentfuel >= engine.FuelPerSecond * Time.deltaTime && generator.CurrentEnergy >= engine.EnergyCost * Time.deltaTime)
        {
            velocity += (Vector2)transform.up * amount * engine.MainThrust * Time.deltaTime;
            frame.Currentfuel -= engine.FuelPerSecond * Time.deltaTime;
            generator.CurrentEnergy -= engine.EnergyCost * Time.deltaTime;
            engineVisability = amount;
        }
    }

    public void ClearInventory()
    {
        frame.ClearInventory();
    }

    public void AddForce(Vector2 force)
    {
        velocity += force;
    }

    public void Repair(int amount = 1)
    {
        frame.Repair(amount);
    }

    public void Refuel(int amount = 1)
    {
        frame.Refuel(amount);
    }

    public bool AddResource(ResourceType r)
    {
        return frame.AddResource(r);
    }

    public bool RemoveResource(ResourceType r, int amount = 1)
    {
        return frame.RemoveResource(r, amount);
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
                p.transform.parent = transform.parent;
                    RaycastHit2D hit = Physics2D.CircleCast(transform.position + transform.up * 2, 1, transform.up, 20, targetable);
                if (hit)
                {
                    p.GetComponent<Projectile>().SetProperties(hit.transform, this);
                }
                else
                {
                    p.GetComponent<Projectile>().SetProperties(transform.up, this);
                }
            }
        }
    }

    public void FirePrimary(bool newPress)
    {
        FireWeapon(newPress, primaryWeapon, weaponSpawn);
    }

    public void FireSecondary(bool newPress)
    {
        FireWeapon(newPress, secondaryWeapon, weaponSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if(velocity.magnitude > engine.MaxVelocity) { velocity = velocity.normalized * engine.MaxVelocity; }
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
            engineAudio.volume = engineVisability;
            engineSprite.color = new Color(1,1,1,engineVisability);
            engineVisability = 0;
        }
        else
        {
            engineAudio.volume = 0;
            engineSprite.color = new Color(1, 1, 1, 0);
        }

        if(shieldVisability > 0)
        {
            shieldAudio.volume = shieldVisability / 10;
            shieldSprite.color = new Color(1, 1, 1, shieldVisability/10);
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
            if(gameObject.GetComponent<Player>() != null){
                collision.gameObject.GetComponent<Station>().OpenStation();
                transform.position = collision.transform.position + (transform.position - collision.transform.position).normalized * 4;
                velocity = Vector2.zero;
            }
            else if(gameObject.GetComponent<AI>() != null && gameObject.GetComponent<AI>().Intent == AI.Intents.Station)
            {
                gameObject.GetComponent<AI>().Trade(collision.gameObject.GetComponent<Station>());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ship")
        {
            AddForce((transform.position - collision.transform.position)/5);
        }
    }

    public void TakeDamage(float amount, Vector2 source, Ship from = null)
    {
        if(from != null && GetComponent<AI>() != null)
        {
            GetComponent<AI>().React(AI.Intents.Fight, from.gameObject);
        }
        shieldSprite.transform.LookAt(source, Vector3.back);
        shieldSprite.transform.rotation = Quaternion.Euler(0, 0, shieldSprite.transform.rotation.eulerAngles.z);

        if(shield.CurrentShield > amount)
        {
            shield.CurrentShield -= amount;
            shieldVisability += amount;
            if (!shieldAudio.isPlaying) { shieldAudio.Play(); }
            shieldAudio.pitch = Random.Range(.5f, 1.5f);
            if(shieldVisability > 10) { shieldVisability = 10; }
        }
        else
        {
            amount -= shield.CurrentShield;
            shieldVisability = 1;
            shield.CurrentShield = 0;
            frame.CurrentHull -= amount;

            if(Hull < 0)
            {
                Destruction();
            }
        }
    }

    private void Destruction()
    {
        foreach(KeyValuePair<ResourceType, int> data in frame.Cargo)
        {
            for (int i = 0; i < data.Value; i++)
            {
                Instantiate(resourceDrop, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), 0), Quaternion.identity).GetComponent<Resource>().Data = data.Key;
            }
        }
        Instantiate(explosion, transform.position - Vector3.back * 2, transform.rotation);
        Destroy(gameObject);
    }
}
