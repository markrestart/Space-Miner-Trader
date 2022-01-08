using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Vector2 velocity;
    [SerializeField]
    public float turnThrust, mainThrust, brakeThrust, maxVelocity, fuelPerSecond, maxShield, shieldRegenPerSecond;
    [SerializeField]
    SpriteRenderer engine, shieldSprite;

    [SerializeField]
    float fuel, hull, shield, cargo;
    float engineVisability, shieldVisability;

    [SerializeField]
    GameObject primaryWeapon, secondaryWeapon;
    [SerializeField]
    float primaryCooldown, secondaryCooldown;
    [SerializeField]
    bool primaryAuto, secondaryAuto;
    [SerializeField]
    Transform primarySpawn, secondarySpawn;
    float lastPrimaryFire, lastSecondaryFire;

    public float Fuel { get => fuel; }
    public float Hull { get => hull; }
    public float Shield { get => shield; }
    public float Cargo { get => cargo; }
    public float Speed { get => velocity.magnitude; }
    public Vector2 Velocity { get => velocity; }

    public void Turn(float amount)
    {
        transform.Rotate(Vector3.back * amount * turnThrust * Time.deltaTime);
    }

    public void Brake()
    {
        velocity = velocity.normalized * (velocity.magnitude - brakeThrust * Time.deltaTime);
    }

    public void Accelerate(float amount)
    {
        if (fuel > 0)
        {
            velocity += (Vector2)transform.up * amount * mainThrust * Time.deltaTime;
            fuel -= fuelPerSecond * Time.deltaTime;
            engineVisability = amount;
        }
    }

    public void FirePrimary(bool newPress)
    {
        if(Time.time > lastPrimaryFire + primaryCooldown)
        {
            if(newPress || primaryAuto)
            {
                lastPrimaryFire = Time.time;
                GameObject p = Instantiate(primaryWeapon, primarySpawn.position, primarySpawn.rotation);
                if(p.GetComponent<Missile>() != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.forward * 2, transform.forward);
                    if (hit)
                    {
                        p.GetComponent<Missile>().SetTarget(hit.transform);
                    }
                }
            }
        }
    }

    public void FireSecondary(bool newPress)
    {
        if (Time.time > lastSecondaryFire + secondaryCooldown)
        {
            if (newPress || secondaryAuto)
            {
                lastSecondaryFire = Time.time;
                GameObject p = Instantiate(secondaryWeapon, secondarySpawn.position, secondarySpawn.rotation);
                if (p.GetComponent<Missile>() != null)
                {
                    RaycastHit2D hit = Physics2D.CircleCast(transform.position + transform.up * 3, 1, transform.up);
                    if (hit)
                    {
                        p.GetComponent<Missile>().SetTarget(hit.transform);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(velocity.magnitude > maxVelocity) { velocity = velocity.normalized * maxVelocity; }
        transform.Translate(velocity * Time.deltaTime, Space.World);

        ShieldRegen();
        Effects();
    }

    void ShieldRegen()
    {
        if(shield < maxShield)
        {
            shield += shieldRegenPerSecond * Time.deltaTime;
        }
        if(shield > maxShield)
        {
            shield = maxShield;
        }
    }

    void Effects() {
        if (engineVisability > 0)
        {
            engine.color = new Color(1,1,1,engineVisability);
        }
        else
        {
            engine.color = new Color(1, 1, 1, 0);
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
        }
    }

    public void TakeDamage(float amount, Vector2 source)
    {
        shieldSprite.transform.LookAt(source, Vector3.back);
        shieldSprite.transform.rotation = Quaternion.Euler(0, 0, shieldSprite.transform.rotation.eulerAngles.z);

        if(shield > amount)
        {
            shield -= amount;
            shieldVisability += 1;
        }
        else
        {
            amount -= shield;
            shieldVisability = 1;
            shield = 0;
            hull -= amount;
        }
    }
}
