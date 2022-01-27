using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float timer = 3, force;

    SpriteRenderer render;


    float startTime;
    Vector3 startScale;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();

        startScale = transform.localScale;
        startTime = timer;
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        render.color = Color.Lerp(Color.clear, Color.white, timer / startTime);
        transform.localScale = Vector3.Lerp(Vector3.zero, startScale, (startTime - timer) / startTime);

        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Resource")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position).normalized * force * ((startTime - timer) / startTime));
        }
        else if(collision.gameObject.tag == "Ship")
        {
            collision.gameObject.GetComponent<Ship>().AddForce((collision.transform.position - transform.position).normalized * force * ((startTime - timer) / startTime));
        }
    }
}
