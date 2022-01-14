using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile
{
    [SerializeField]
    float speed, power;

    public override void SetProperties(Transform t, Ship source)
    {
        this.source = source;
        transform.up = Vector2.Lerp(transform.up, t.position - transform.position, .5f);
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    public override void SetProperties(Vector2 aim, Ship source)
    {
        this.source = source;
        transform.up = aim;
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            collision.gameObject.GetComponent<Asteroid>().TakeDamge(power);
        }else if(collision.gameObject.tag == "Ship")
        {
            collision.gameObject.GetComponent<Ship>().TakeDamage(power, transform.position);
        }

        Destroy(gameObject);
    }

}
