using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    float speed, power;

    // Start is called before the first frame update
    void Start()
    {
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
