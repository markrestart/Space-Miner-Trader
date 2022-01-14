using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Ship source;

    public abstract void SetProperties(Transform t, Ship source);

    public abstract void SetProperties(Vector2 aim, Ship source);
}
