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
    GameObject resource, explosion;
    [SerializeField]
    ResourceOverlay overlay;

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
                a1.transform.parent = transform.parent;
                GameObject a2 = Instantiate(gameObject, transform.position, transform.rotation);
                a2.GetComponent<Asteroid>().SetStats(s2, style);
                a2.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4, 4), Random.Range(-4, 4)));
                a2.transform.parent = transform.parent;
            }
            else
            {
                Instantiate(explosion, transform.position, transform.rotation);
                for (int i = 0; i < size; i++)
                {
                    GameObject r = Instantiate(resource, transform.position + new Vector3(Random.Range(-.3f, .3f), Random.Range(-.3f, .3f)), transform.rotation);
                    r.GetComponent<Resource>().Data = ResourceLedger.RTs[style];
                    r.transform.parent = transform.parent;
                }
            }

            Destroy(gameObject);
        }
    }


    private void OnMouseEnter()
    {
        overlay.Initialize(ResourceLedger.RTs[style]);
        overlay.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        overlay.gameObject.SetActive(false);
    }
}
