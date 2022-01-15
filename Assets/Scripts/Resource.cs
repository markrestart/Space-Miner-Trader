using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField]
    ResourceType data;

    public ResourceType Data { get => data; set {
            data = value;
            GetComponent<SpriteRenderer>().sprite = data.image;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Data = ResourceLedger.RTs[Random.Range(0, ResourceLedger.RTs.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
