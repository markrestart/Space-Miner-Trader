using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<Sprite> resourceSprites;

    // Start is called before the first frame update
    void Start()
    {
        ResourceLedger.Initialize(resourceSprites);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
