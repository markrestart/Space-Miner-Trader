using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLedger 
{
    static string[] prefix = new string[] {"Giga","Hydro", "Hyper", "Vibra", "Black", "Red", "Blue", "Invisa", "nTH", "Cryo", "Omni", "Cyber", "Arso","Krypto","Kyber","Myth","Na","Omi","Proto","Quanti","Trita" };
    static string[] suffix = new string[] { "metal", "mineral", "tium", "nium", "crystal", "diamond", "saphire", "ruby", "-99","-52","-42","-451","nite","ril","cron","dium","stone"};

    public static void Initialize(List<Sprite> sprites)
    {
        RTs = new List<ResourceType>();
        for(int i = 0; i < sprites.Count; i++)
        {
            Sprite sprite = sprites[i];
            int weight = Random.Range(1, 10);
            int valuePoints = 10 + weight;
            int bioValue = 0, mechValue = 0, techValue = 0, intrinsicValue = 0;
            while(valuePoints > 0)
            {
                int rand = Random.Range(0,4);
                switch (rand)
                {
                    case 1:
                        bioValue++;
                        break;
                    case 2:
                        techValue++;
                        break;
                    case 3:
                        mechValue++;
                        break;
                    default:
                        intrinsicValue++;
                        break;
                }

                valuePoints--;
            }

            string name = prefix[Random.Range(0, prefix.Length)] + suffix[Random.Range(0, suffix.Length)];

            RTs.Add(new ResourceType(weight, name, bioValue, mechValue, techValue, intrinsicValue, sprite));
        }
    }

    public static List<ResourceType> RTs;
}

public struct ResourceType
{
    public float weight;
    public string name;

    public int bioValue;
    public int mechValue;
    public int techValue;
    public int intrinsicValue;

    public Sprite image;

    public ResourceType(float weight, string name, int bioValue, int mechValue, int techValue, int intrinsicValue, Sprite image)
    {
        this.weight = weight;
        this.name = name;
        this.bioValue = bioValue;
        this.mechValue = mechValue;
        this.techValue = techValue;
        this.intrinsicValue = intrinsicValue;
        this.image = image;
    }
}
