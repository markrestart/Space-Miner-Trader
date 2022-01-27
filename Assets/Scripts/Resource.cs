using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField]
    ResourceType data;
    [SerializeField]
    ResourceOverlay overlay;

    public ResourceType Data { get => data; set {
            data = value;
            GetComponent<SpriteRenderer>().sprite = data.image;
            overlay.Initialize(data);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(data.image == null)
        {
            Data = ResourceLedger.RTs[Random.Range(0, ResourceLedger.RTs.Count)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseEnter()
    {
        overlay.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        overlay.gameObject.SetActive(false);
    }
}
