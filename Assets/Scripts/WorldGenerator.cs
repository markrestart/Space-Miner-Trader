using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    List<SpawnStats> objects;
    [SerializeField]
    GameObject blankChunk;

    float minX, maxX, minY, maxY;
    int x, y;
    bool activeChunk;

    private static List<WorldGenerator> map;
    private static Transform player;

    public int X { get => x; }
    public int Y { get => y; }

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>().transform;
        }
        if(map == null)
        {
            map = new List<WorldGenerator>();
            BecomeActive();
        }
        map.Add(this);

        minX = transform.position.x - transform.lossyScale.x / 2;
        maxX = transform.position.x + transform.lossyScale.x / 2;
        minY = transform.position.y - transform.lossyScale.y / 2;
        maxY = transform.position.y + transform.lossyScale.y / 2;

        float size = (maxX - minX) * (maxY - minY)/1000;

        foreach(SpawnStats pair in objects)
        {
            int amount = Mathf.RoundToInt(size * pair.density);
            for(int i = 0; i<amount; i++)
            {
                Transform g = Instantiate(pair.spawn, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), Quaternion.Euler(0, 0, Random.Range(0, 360))).transform;
                g.parent = transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activeChunk && player != null)
        {
            if(player.transform.position.x < minX)
            {
                map.FindLast(n => n.X == x - 1 && n.y == y).BecomeActive();
                activeChunk = false;
            }

            if (player.transform.position.x > maxX)
            {
                map.FindLast(n => n.X == x + 1 && n.y == y).BecomeActive();
                activeChunk = false;
            }

            if (player.transform.position.y < minY)
            {
                map.FindLast(n => n.X == x && n.y == y - 1).BecomeActive();
                activeChunk = false;
            }

            if (player.transform.position.y > maxY)
            {
                map.FindLast(n => n.X == x && n.y == y + 1).BecomeActive();
                activeChunk = false;
            }
        }

        foreach(Transform t in transform)
        {
            if (t.position.x < minX)
            {
                if (map.FindLast(n => n.X == x - 1 && n.y == y) != null)
                {
                    t.transform.parent = map.FindLast(n => n.X == x - 1 && n.y == y).transform;
                }
                else
                {
                    Destroy(t.gameObject);
                }
            }

            if (t.position.x > maxX)
            {
                if (map.FindLast(n => n.X == x + 1 && n.y == y) != null)
                {
                    t.transform.parent = map.FindLast(n => n.X == x + 1 && n.y == y).transform;
                }
                else
                {
                    Destroy(t.gameObject);
                }
            }

            if (t.position.y < minY)
            {
                if (map.FindLast(n => n.X == x && n.y == y - 1) != null)
                {
                    t.transform.parent = map.FindLast(n => n.X == x && n.y == y - 1).transform;
                }
                else
                {
                    Destroy(t.gameObject);
                }
            }

            if (t.position.y > maxY)
            {
                if (map.FindLast(n => n.X == x && n.y == y + 1) != null)
                {
                    t.transform.parent = map.FindLast(n => n.X == x && n.y == y + 1).transform;
                }
                else
                {
                    Destroy(t.gameObject);
                }
            }
        }
    }

    void CheckNeighbors() { 
        if(!map.Exists(n => n.X == x-1 && n.y == y)) { MakeChunk(new Vector2(transform.position.x - transform.lossyScale.x, transform.position.y), X - 1, Y); }
        if (!map.Exists(n => n.X == x + 1 && n.y == y)) { MakeChunk(new Vector2(transform.position.x + transform.lossyScale.x, transform.position.y), X + 1, Y); }
        if (!map.Exists(n => n.X == x - 1 && n.y == y + 1)) { MakeChunk(new Vector2(transform.position.x - transform.lossyScale.x, transform.position.y + transform.lossyScale.y), X - 1, Y + 1); }
        if (!map.Exists(n => n.X == x - 1 && n.y == y - 1)) { MakeChunk(new Vector2(transform.position.x - transform.lossyScale.x, transform.position.y - transform.lossyScale.y), X - 1, Y - 1); }
        if (!map.Exists(n => n.X == x + 1 && n.y == y + 1)) { MakeChunk(new Vector2(transform.position.x + transform.lossyScale.x, transform.position.y + transform.lossyScale.y), X + 1, Y + 1); }
        if (!map.Exists(n => n.X == x + 1 && n.y == y - 1)) { MakeChunk(new Vector2(transform.position.x + transform.lossyScale.x, transform.position.y - transform.lossyScale.y), X + 1, Y - 1); }
        if (!map.Exists(n => n.X == x && n.y == y + 1)) { MakeChunk(new Vector2(transform.position.x, transform.position.y + transform.lossyScale.y), X, Y + 1); }
        if (!map.Exists(n => n.X == x && n.y == y - 1)) { MakeChunk(new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y), X, Y - 1); }
    }

    public void BecomeActive()
    {
        activeChunk = true;
        CheckNeighbors();

        foreach(WorldGenerator chunk in map)
        {
            if(chunk.X >= x-1 && chunk.X <= x+1 && chunk.Y >= y-1 && chunk.Y <= y + 1)
            {
                chunk.gameObject.SetActive(true);
            }
            else
            {
                chunk.gameObject.SetActive(false);
            }
        }
    }

    void MakeChunk(Vector2 position, int setX, int setY)
    {
        Instantiate(blankChunk, new Vector3(position.x,position.y,10), Quaternion.identity).GetComponent<WorldGenerator>().SetXY(setX, setY);
    }
    void SetXY(int setX, int setY)
    {
        x = setX;
        y = setY;
        name = "(" + x + "," + y + ")";
    }
}

[System.Serializable]
public struct SpawnStats
{
    public GameObject spawn;
    public float density;
}
