using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    List<SpawnStats> objects;

    float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
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
                Instantiate(pair.spawn, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

[System.Serializable]
public struct SpawnStats
{
    public GameObject spawn;
    public float density;
}
