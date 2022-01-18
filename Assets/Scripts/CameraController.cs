using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Ship target;
    [SerializeField]
    float minScale, maxScale;
    [SerializeField] [Range(0, 1)]
    float moveLerp, scaleLerp;
    [SerializeField]
    [Range(.1f, 5)]
    float scaleMultiplier;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = target.Speed;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,Mathf.Clamp(speed, minScale, maxScale) * scaleMultiplier, scaleLerp);
        Vector3 goal = new Vector3(target.transform.position.x + target.Velocity.x / 2, target.transform.position.y + target.Velocity.y / 2, -10);
        transform.position = Vector3.Lerp(transform.position, goal, moveLerp);
    }
}
