using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    float timeToFire, thrust, turn, power;
    float launchTime;
    bool active;
    Rigidbody2D rb;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        launchTime = Time.time;

        rb.AddForce(new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)), ForceMode2D.Impulse);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            float dir = Vector3.Dot(Vector3.Cross(transform.up, (transform.position - target.position).normalized), Vector3.back);

            if (dir > 0f)
            {
                rb.AddTorque(turn);
            }
            else if (dir < 0f)
            {
                rb.AddTorque(-turn);
            }
        }

        if (active)
        {
            rb.AddForce(transform.up * thrust);

        }else if(Time.time > timeToFire + launchTime)
        {
            active = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (active)
        {

            if (collision.gameObject.tag == "Asteroid")
            {
                collision.gameObject.GetComponent<Asteroid>().TakeDamge(collision.relativeVelocity.magnitude * power);
            }
            else if (collision.gameObject.tag == "Ship")
            {
                float relativeVelocity = (collision.gameObject.GetComponent<Ship>().Velocity - rb.velocity).magnitude;
                collision.gameObject.GetComponent<Ship>().TakeDamage(relativeVelocity * power, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
