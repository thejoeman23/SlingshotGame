using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] TurnScript turnScript;
    [SerializeField] PointSystem pointSystem;

    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float forceMultiplier = 5;

    public bool isPlayerOne;
    public bool stretched = false;
    public bool launched = false;
    public float gravity = 1f; // Fixed gravity
    public int resolution = 10; // Fixed number of points for the trajectory
    public float objectMass = 100f; // Mass of each object

    private Vector2 direction;
    private Vector2 firstClicked;
    private Vector2 bandPosition;
    public int stillnessThreshold = 200; // Number of frames the tower has to be still before ending the turn
    private float band;
    private int stillness = 0; // The number of frames since the tower moved last

    private GameObject currentObject;
    private GameObject nextObject;

    SpriteRenderer sr;

    List<GameObject> trajectoryPoints = new List<GameObject>();
    [SerializeField] GameObject trajectoryPointPrefab; // The prefab for trajectory points

    [SerializeField] Animator animator;

    private void Start()
    {
        // Sets current object and next object to a random object in the list
        currentObject = objects[UnityEngine.Random.Range(0, objects.Count)];
        nextObject = objects[UnityEngine.Random.Range(0, objects.Count)];

        bandPosition = transform.position;
        if (transform.parent.name == "Player 1") isPlayerOne = true;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // The sprite renderer must always represent the current object
        sr.sprite = currentObject.GetComponent<SpriteRenderer>().sprite;
        sr.color = currentObject.GetComponent<SpriteRenderer>().color;
        transform.localScale = currentObject.transform.localScale;
        transform.position = bandPosition;

        Vector2 cursorScreenPos = Input.mousePosition;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);

        if (launched) // launched phase
        {
            animator.SetBool("Shot", true);
            animator.SetBool("IsHolding", false);

            // The tower is an array of all the launched gameObjects
            GameObject[] tower = GameObject.FindGameObjectsWithTag("projectile");

            // Destroy all blocks that are out of bounds and add points
            GameObject[] fallen = fallenObj(tower);
            if (fallen.Count() > 0)
            {
                pointSystem.addPoints(isPlayerOne, fallen.Count());
                foreach (GameObject obj in fallen) Destroy(obj);
                return; // Go to next update cycle to update the tower array
            }

            if (towerMoving(tower)) return;
            else if (stillness < stillnessThreshold) { stillness++; return; }

            turnOver();
        }
        else // stretched phase
        {
            if (Input.GetMouseButtonDown(0)) { firstClicked = cursorPos; stretched = true; }
            if (stretched)
            {
                stretchTo(cursorPos);
                animator.SetBool("IsHolding", true);
                animator.SetBool("Shot", false);
            }
        }
    }

    // Resets all variables and ends the current player's turn
    private void turnOver()
    {
        stillness = 0;
        launched = false;
        turnScript.switchTurns();
    }

    // Draw a line on the launcher representing the vector between firstClicked and the given position
    void stretchTo(Vector2 pos)
    {
        band = Vector2.Distance(firstClicked, pos);
        direction = (firstClicked - pos).normalized; // The direction the object will be launched in

        // Create band; bandPosition is where the projectile sits
        bandPosition = (Vector2)lineRenderer.gameObject.transform.position + (direction * band * -1);
        lineRenderer.SetPosition(1, bandPosition);

        // Calculate and display the trajectory using gameObjects
        List<Vector2> trajectoryPositions = CalculateTrajectory(transform.position, direction, band, forceMultiplier);
        ShowTrajectory(trajectoryPositions); // New function for trajectory visualization

        if (Input.GetMouseButtonUp(0))
        {
            stretched = false;

            lineRenderer.SetPosition(1, lineRenderer.gameObject.transform.position);
            ClearTrajectory(); // Clears the trajectory when launching

            launch(direction, band);
        }
    }

    void launch(Vector2 direction, float force)
    {
        // Instantiates the current object
        GameObject newObject = Instantiate(currentObject);
        newObject.tag = "projectile";
        newObject.transform.position = transform.position;

        // Set the Rigidbody2D's velocity using the calculated direction and force
        newObject.GetComponent<Rigidbody2D>().linearVelocity = direction * force * forceMultiplier;

        // Sets a new next object and updates the current one
        currentObject = nextObject;
        nextObject = objects[UnityEngine.Random.Range(0, objects.Count)];

        // Return to the ready to launch position
        bandPosition = transform.parent.position + Vector3.up;
        transform.position = transform.parent.position + Vector3.up;

        launched = true;
    }

    // Function to calculate trajectory points
    public List<Vector2> CalculateTrajectory(Vector2 startPosition, Vector2 direction, float force, float forceMultiplier)
    {
        List<Vector2> trajectoryPoints = new List<Vector2>();

        // Halve the force applied to the initial velocity
        float scalingFactor = 0.5f;
        Vector2 initialVelocity = direction * force * forceMultiplier * scalingFactor;

        // Total time until the projectile hits the ground
        float totalTime = CalculateFlightTime(initialVelocity.y);

        // Calculate trajectory points over time (fixed resolution of 10)
        float timeStep = totalTime / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float time = i * timeStep;
            Vector2 point = CalculatePositionAtTime(startPosition, initialVelocity, time);
            trajectoryPoints.Add(point);
        }

        return trajectoryPoints;
    }


    // Function to calculate flight time based on initial vertical velocity
    private float CalculateFlightTime(float initialVerticalVelocity)
    {
        // The formula to calculate the time until the object hits the ground
        return (2 * initialVerticalVelocity) / gravity;
    }

    // Function to calculate the position of the object at a given time
    private Vector2 CalculatePositionAtTime(Vector2 startPosition, Vector2 initialVelocity, float time)
    {
        float x = startPosition.x + initialVelocity.x * time;
        float y = startPosition.y + initialVelocity.y * time - 0.5f * gravity * time * time;

        return new Vector2(x, y);
    }

    // Function to instantiate gameObjects along the trajectory
    void ShowTrajectory(List<Vector2> points)
    {
        // Clear the previous frame's trajectory points
        ClearTrajectory();

        // Instantiate a new trajectory point at each calculated position
        foreach (Vector2 point in points)
        {
            GameObject trajectoryPoint = Instantiate(trajectoryPointPrefab, point, Quaternion.identity);
            trajectoryPoints.Add(trajectoryPoint);
        }
    }

    // Function to clear previously instantiated trajectory points
    void ClearTrajectory()
    {
        foreach (GameObject point in trajectoryPoints)
        {
            Destroy(point);
        }
        trajectoryPoints.Clear();
    }

    // Returns false if the combined magnitude of all objects is greater than threshold
    bool towerMoving(GameObject[] tower)
    {
        var threshold = 0;
        var sumOfMagnitude = 0;
        foreach (GameObject obj in tower)
            sumOfMagnitude += (int)obj.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        return sumOfMagnitude > threshold;
    }

    // Returns an array of all the projectiles which are out of bounds
    GameObject[] fallenObj(GameObject[] tower)
    {
        var buffer = 2; // So that objects can be stretched off the screen without ending the turn early
        var fallen = new List<GameObject>();
        foreach (GameObject block in tower)
            if (block.transform.position.x < -9 * buffer || block.transform.position.x > 9 * buffer ||
                block.transform.position.y < -5 * buffer
            ) fallen.Add(block);
        return fallen.ToArray();
    }
}