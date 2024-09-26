using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] GameObject spawner;

    public void spawn()
    {
        GameObject newObject = Instantiate(objects[Random.Range(0, objects.Count)]);
        newObject.transform.position = spawner.transform.position;
    }
}
