using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float goalDistance;

    Ship self;

    public Ship Self { get => self; }

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Ship>();
    }

    Vector2 goal;
    GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target == null) { target = FindObjectOfType<Player>().gameObject; }
        goal = target.transform.position + (transform.position - target.transform.position).normalized * goalDistance;

        Vector2 desiredVelocity = (goal - (Vector2)transform.position).normalized * (Vector2.Distance(transform.position, goal) * self.Engine.BrakeThrust);
        Vector2 desiredDirection = desiredVelocity - self.Velocity;

        switch (Helpers.RightOrLeft(transform, transform.position + (Vector3)desiredVelocity))
        {
            case 1:
                self.Turn(-1);
                break;
            case 2:
                self.Turn(1);
                break;
        }
        float aimAlignment = Vector2.Dot(transform.up.normalized, desiredDirection.normalized);
        float velocityAlignment = Vector2.Dot(desiredVelocity, self.Velocity);

        if(aimAlignment > 0f)
        {
            if(self.Speed == 0 ||( velocityAlignment > 0 && self.Velocity.magnitude <= desiredVelocity.magnitude))
            {
                self.Accelerate(aimAlignment);
            }
            else
            {
                self.Brake();
            }

            if (Vector2.Distance(transform.position, goal) < 5)
            {
                self.FireSecondary(Random.Range(0, 4) > 2);
            }
            else if (Vector2.Distance(transform.position, goal) < 8)
            {
                self.FirePrimary(false);
            }

        }else if(aimAlignment < 0 && (velocityAlignment < 0 || self.Velocity.magnitude > desiredVelocity.magnitude))
        {
            self.Brake();
        }   
    }
}
