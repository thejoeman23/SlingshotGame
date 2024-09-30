using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.WSA;

public class Cursor : MonoBehaviour
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float forceMultiplier = 2;
    [SerializeField] float blockScale = 1;

    Vector2 direction;
    Vector2 firstClicked;
    public bool stretched = false;
    float band;
    Vector2 bandPosition;

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
        transform.position = bandPosition; 

        // updates the spawner position to the cursor's position
        Vector2 cursorScrenPos = Input.mousePosition;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(cursorScrenPos);

        // when player holds mouse button to aim, this is where they first click
        if (Input.GetMouseButtonDown(0)) { firstClicked = cursorPos; stretched = true; }

	    if (stretched) stretchTo(cursorPos);
    }

    // draw a line on the launcher representing the vector between firstClicked and the given position
    void stretchTo(Vector2 pos) {

        band = Vector2.Distance(firstClicked, pos);
        direction = (firstClicked - pos).normalized; // the direction the object will be launched in

        bandPosition = (Vector2)lineRenderer.gameObject.transform.position + (direction * band * -1);

        lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
        lineRenderer.SetPosition(1, bandPosition);

        if (Input.GetMouseButtonUp(0))
        {
            stretched = false;

            lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
            lineRenderer.SetPosition(1, lineRenderer.gameObject.transform.position);

            launch(direction, band);
        }
    }

    void launch(Vector2 direction, float force)
    {
        // instantiates the current object
        GameObject newObject = Instantiate(currentObject);
        newObject.transform.position = transform.position;
        newObject.GetComponent<Rigidbody2D>().linearVelocity = direction * force * forceMultiplier;

        // sets a new next object and updates the current one;
        currentObject = nextObject;
        nextObject = objects[Random.Range(0, objects.Count)];

	    // scale into a random shape, 0.5 minimum
     	nextObject.transform.localScale = new Vector2(0.5f + Random.value * blockScale, 0.5f + Random.value* blockScale);

        // return to the ready to launch position
        bandPosition = transform.parent.position;
        transform.position = transform.parent.position;
    }
}
