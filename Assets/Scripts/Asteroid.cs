using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    int style;
    float size, health;
    [SerializeField]
    List<Sprite> sprites;
    [SerializeField]
    GameObject resource;

    bool created;

    // Start is called before the first frame update
    void Start()
    {
        if (!created)
        {
            size = Random.Range(1, 8);
            health = Random.Range(1, 14) * size;
            style = Random.Range(0, sprites.Count);

            transform.localScale = new Vector3(size / transform.parent.localScale.x, size / transform.parent.localScale.y, size / transform.parent.localScale.z);
            GetComponent<SpriteRenderer>().sprite = sprites[style];
        }
    }

    public void SetStats(float set_size, int set_style)
    {
        size = set_size;
        style = set_style;
        health = Random.Range(1, 14) * size;

        transform.localScale = new Vector3(size, size, size);
        GetComponent<SpriteRenderer>().sprite = sprites[style];

        created = true;
    }

    public void TakeDamge(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            if(size > 5)
            {
                float s1 = Random.Range(2, 4);
                float s2 = size - s1;
                GameObject a1 = Instantiate(gameObject, transform.position, transform.rotation);
                a1.GetComponent<Asteroid>().SetStats(s1, style);
                a1.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4, 4), Random.Range(-4, 4)));
                GameObject a2 = Instantiate(gameObject, transform.position, transform.rotation);
                a2.GetComponent<Asteroid>().SetStats(s2, style);
                a2.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4, 4), Random.Range(-4, 4)));
            }
            else
            {
                Instantiate(resource, transform.position, transform.rotation).GetComponent<Resource>().Data = ResourceLedger.RTs[style];
            }

            Destroy(gameObject);
        }
    }
}
