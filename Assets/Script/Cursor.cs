using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] bool isHolding = false;
    [SerializeField] float forceMultiplier = 2;
    float force;

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
        Vector3 cursorScrenPos = Input.mousePosition;
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(cursorScrenPos);

        // when player holds mouse button to aim, this is where they first click
        Vector3 firstClicked = Vector3.zero;
        if (Input.GetMouseButtonDown(0)) { firstClicked = cursorPos; }

        // the direction the object will be launched in
        Vector2 direction = (firstClicked - cursorPos).normalized;

        while (Input.GetMouseButton(0))
        {
            isHolding = true;

            force = Vector2.Distance(firstClicked, cursorPos) * forceMultiplier;

            lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position + ((Vector3)direction * force));
            lineRenderer.SetPosition(1, lineRenderer.gameObject.transform.position);

            return;
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            isHolding = false;

            lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
            lineRenderer.SetPosition(1, lineRenderer.gameObject.transform.position);

            launch(direction);
        }
    }

    void launch(Vector2 direction)
    {
        // instantiates the current object
        GameObject newObject = Instantiate(currentObject);
        newObject.transform.position = transform.position;
        newObject.GetComponent<Rigidbody2D>().linearVelocity = direction * force * forceMultiplier;

        // sets a new next object and updates the current one;
        currentObject = nextObject;
        nextObject = objects[Random.Range(0, objects.Count)];
    }
}
