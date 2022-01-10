using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField]
    ResourceType data;

    public ResourceType Data { get => data; }

    // Start is called before the first frame update
    void Start()
    {
        data = ResourceLedger.RTs[Random.Range(0, ResourceLedger.RTs.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
