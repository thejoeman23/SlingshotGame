using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] float cursorHeight = 3;

    GameObject currentObject;
    GameObject nextObject;

    SpriteRenderer sr;

    private void Start()
    {
        // sets current object and next object to a random object in the list
        currentObject = objects[Random.Range(0, objects.Count)];
        nextObject = objects[Random.Range(0, objects.Count)];

        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // the sprite renderer must always represent the current object;
        sr.sprite = currentObject.GetComponent<SpriteRenderer>().sprite;
        sr.color = currentObject.GetComponent<SpriteRenderer>().color;
        transform.localScale = currentObject.transform.localScale;

        // updates the spawner position to the cursor's position
        Vector3 cursorPos = Input.mousePosition;
        float cursorXPos = Camera.main.ScreenToWorldPoint(cursorPos).x;

        transform.position = new Vector3(cursorXPos, cursorHeight, 0);

        if (Input.GetMouseButtonDown(0))
        {
            spawn();
        }
    }

    void spawn()
    {
        // instantiates the current object
        GameObject newObject = Instantiate(currentObject);
        newObject.transform.position = transform.position;

        // sets a new next object and updates the current one;
        currentObject = nextObject;
        nextObject = objects[Random.Range(0, objects.Count)];
    }
}
