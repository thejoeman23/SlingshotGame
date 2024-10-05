using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.WSA;

public class Cursor : MonoBehaviour
{
    [SerializeField] TurnScript turnScript;
    [SerializeField] PointSystem pointSystem;

    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float forceMultiplier = 5;

    Vector2 direction;
    Vector2 firstClicked;
    public int myPoints = 0;
    public bool isPlayerOne;
    public bool stretched = false;
    public bool launched = false;
    float band;
    Vector2 bandPosition;

    GameObject currentObject;
    GameObject nextObject;

    SpriteRenderer sr;

    private void Start()
    {
        // sets current object and next object to a random object in the list
        currentObject = objects[UnityEngine.Random.Range(0, objects.Count)];
        nextObject = objects[UnityEngine.Random.Range(0, objects.Count)];

        bandPosition = transform.position;

        if (transform.parent.name == "Player 1") isPlayerOne = true;

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

        Vector2 cursorScreenPos = Input.mousePosition;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);

        if (launched)
        {
            // the tower is an array of all the launched gameObjects
            GameObject[] tower = GameObject.FindGameObjectsWithTag("projectile");
            
            // destroy all blocks that are out of bounds
            GameObject[] fallen = fallenObj(tower);
            if (fallen.Count() > 0) {
                myPoints += fallen.Count();
                pointSystem.addPoints(isPlayerOne, fallen.Count());
                foreach (GameObject obj in fallen) Destroy(obj);
                return; // go to next update cycle to update the tower array
            }

            if (towerMoving(tower)) return; // wait for movement to stop by going to the next update cycle

            // turn is over because all projectiles are in bounds, and the tower isn't moving
            launched = false;
            turnScript.switchTurns();
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) { firstClicked = cursorPos; stretched = true; }
            if (stretched) stretchTo(cursorPos);
        }
    }

    // draw a line on the launcher representing the vector between firstClicked and the given position
    void stretchTo(Vector2 pos)
    {
        band = Vector2.Distance(firstClicked, pos);
        direction = (firstClicked - pos).normalized; // the direction the object will be launched in

        // create band, bandPosition is where the projectile sits
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
        newObject.tag = "projectile";
        newObject.transform.position = transform.position;
        newObject.GetComponent<Rigidbody2D>().linearVelocity = direction * force * forceMultiplier;

        // sets a new next object and updates the current one;
        currentObject = nextObject;
        nextObject = objects[UnityEngine.Random.Range(0, objects.Count)];

        // return to the ready to launch position
        bandPosition = transform.parent.position + Vector3.up;
        transform.position = transform.parent.position + Vector3.up;

        launched = true;
    }

    // returns false if the combined magnitude of all objects is greater than threshold
    bool towerMoving(GameObject[] tower)
    {
        var threshold = 0;
        var sumOfMagnitude = 0;
        foreach (GameObject obj in tower)
            sumOfMagnitude += (int)obj.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        if (sumOfMagnitude > threshold) return true;
        else return false;
    }

    // returns an array of all the projectiles which are out of bounds
    GameObject[] fallenObj(GameObject[] tower)
    {
        var buffer = 2; // so that objects can be stretched off the screen without ending the turn early
        var fallen = new List<GameObject>();
        foreach (GameObject block in tower)
            if (block.transform.position.x < -9 * buffer || block.transform.position.x > 9 * buffer || 
                block.transform.position.y < -5 * buffer 
            ) fallen.Add(block);
        return fallen.ToArray();
    }

    void gameOver()
    {
    }
}
