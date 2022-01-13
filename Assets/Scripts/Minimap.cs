using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField]
    GameObject icon;

    [SerializeField]
    Sprite Asteroid, Ship, Station;

    public void UpdateMap(List<MapObject> objects)
    {
        while(objects.Count > transform.childCount)
        {
            Instantiate(icon, Vector2.zero, Quaternion.identity, transform);
        }

        int i = 0;
        foreach(MapObject o in objects)
        {
            Sprite sprite = o.size == 1 ? Asteroid : o.size == 2 ? Ship : Station;
            transform.GetChild(i).GetComponent<Image>().sprite = sprite;
            transform.GetChild(i).localPosition = o.relativePosition;
            i++;
        }
    }

}

public struct MapObject
{
    public Vector2 relativePosition;
    public int size;

    public MapObject(Vector2 pos, int s)
    {
        relativePosition = pos;
        size = s;
    }
}
