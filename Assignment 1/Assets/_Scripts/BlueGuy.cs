using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;

public class BlueGuy : AgentObject
{
<<<<<<< Updated upstream:Assignment 1/Assets/_Scripts/BlueGuy.cs
    public GameObject agentSprite;
    public GameObject targetSprite;

    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    // Add fields for whisper length, angle and avoidance weight.
    [SerializeField] float whiskerLength = 1.5f;
    [SerializeField] float frontWhiskerAngle = 45f;
    [SerializeField] float avoidanceWeight = 2f;
=======
    [SerializeField] float movementSpeed = 3;
    [SerializeField] float rotationSpeed = 60;
    // Add fields for whisper length, angle and avoidance weight.
    [SerializeField] float whiskerLength = 1.5f;
    [SerializeField] float frontWhiskerAngle = 70f;
    [SerializeField] float backWhiskerAngle = 110f;
    [SerializeField] float avoidanceWeight = 3f;
>>>>>>> Stashed changes:Assignment 1/Assets/_Scripts/BlueSeekAvoid.cs
    private Rigidbody2D rb;
    private Vector2 positionA;
    private Vector2 positionT;


    public AudioClip bye;  
    public AudioClip yay; 

    private AudioSource audioSource;

    new void Start() // Note the new.
    {
        base.Start(); // Explicitly invoking Start of AgentObject.
        Debug.Log("Starting Starship.");
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Instantiate targetSprite
            positionT = new Vector2(6f, 1f);
            Instantiate(targetSprite, positionT, Quaternion.identity);

            // Parent
            GameObject parent = GameObject.Find("Parent");

            // Instantiate agentSprite
            Vector3 parentPosition = new Vector2(-6.5f, -2.5f); ;
            parent.transform.position = parentPosition;
            positionA = parentPosition;
            GameObject agent = Instantiate(agentSprite, positionA, Quaternion.identity);

            // Set the parent for agent
            agent.transform.parent = parent.transform;

            Seek();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        { 

            // Flee();
            Flee();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
        else if(!Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
        else if(!Input.GetKeyDown(KeyCode.Alpha5))
        {
            Seek();
            AvoidObstacles();
        }
    }

    private void AvoidObstacles()
    {
        bool hitleft = CastWhisker(frontWhiskerAngle);
        bool hitright = CastWhisker(-frontWhiskerAngle);

        // Adjust rotation based on detected obstacles.
        if (hitleft)
        {
            // Rotate clockwise if the left whisker is hit
            RotateClockwise();
        }
        else if (hitright & !hitleft)
        {
            // Rotate counterclockwise if the right whisker hit
            RotateCounterClockwise();
        }

    }

    private void RotateClockwise()
    {
        // Rotate clockwise based on rotationSpeed and a weight.
        transform.Rotate(Vector3.forward, rotationSpeed * avoidanceWeight * Time.deltaTime);
    }
    private void RotateCounterClockwise()
    {
        // Rotate counterclockwise based on rotationSpeed and a weight.
        transform.Rotate(Vector3.forward, -rotationSpeed * avoidanceWeight * Time.deltaTime);
    }



    private bool CastWhisker(float angle)
    {

        Color rayColor = Color.red;
        bool hitResult = false;
        // Cast whiskers to detect obstacles.
        


        // Calculate the direction of the whisker
        Vector2 whiskerDirection = Quaternion.Euler(0, 0, angle) * transform.up;

        // Cast a ray in the whisker direction;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, whiskerDirection, whiskerLength);

        // Check if the ray hits an obstacle
        if (hit.collider != null)
        {
            Debug.Log("Obstacle Detected!!!");

            rayColor = Color.green;
            hitResult = true;
        }
        Debug.DrawRay(transform.position, whiskerDirection * whiskerLength, rayColor);
        return hitResult;
    }

    private void Seek()
{
    // Update positions based on GameObjects' actual positions
    Vector2 positionA = transform.position;
    Vector2 positionT = GameObject.Find("Target(Clone)").transform.position;

    // Calculate direction to the target.
    Vector2 directionToTarget = (positionT - positionA).normalized;

    // Calculate the angle to rotate towards the target.
    float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90.0f;

    // Smoothly rotate towards the target.
    float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
    float rotationStep = rotationSpeed * Time.deltaTime;
    float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
    transform.Rotate(Vector3.forward, rotationAmount);

    // Move directly towards the target using Rigidbody2D.
    rb.velocity = directionToTarget * movementSpeed;
}
    private void Flee()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            audioSource.PlayOneShot(bye);
        }
        if (other.gameObject.tag == "Target")
        {
            audioSource.PlayOneShot(yay);
        }
    }
}
