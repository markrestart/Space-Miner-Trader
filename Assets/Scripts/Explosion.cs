using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float timer = 3, force;

    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        render.color = Color.Lerp(Color.clear, Color.white, timer / 3);

        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Resource")
        {
            print("Explodion pushing asteroid");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position).normalized * force);
        }
        else if(collision.gameObject.tag == "Ship")
        {
            print("Explosion pushing ship!");
            collision.gameObject.GetComponent<Ship>().AddForce((collision.transform.position - transform.position).normalized * force);
        }
    }
}
