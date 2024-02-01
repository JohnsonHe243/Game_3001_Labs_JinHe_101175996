using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;

public class BlueGuy : AgentObject
{
    public GameObject agent;
    public GameObject target;
    public GameObject obstacle;

    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    // Add fields for whisper length, angle and avoidance weight.
    [SerializeField] float whiskerLength = 1.5f;
    [SerializeField] float frontWhiskerAngle = 45f;
    [SerializeField] float avoidanceWeight = 2f;
    private Rigidbody2D rb;
    private Vector2 positionA;
    private Vector2 positionT;
    private Vector2 positionO;


    new void Start() // Note the new.
    {
        base.Start(); // Explicitly invoking Start of AgentObject.
        Debug.Log("Starting Starship.");
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Instantiate targetSprite
            positionT = new Vector2(6f, 1f);
            Instantiate(target, positionT, Quaternion.identity);

            // Parent
            GameObject parent = GameObject.Find("Parent");

            // Instantiate agentSprite
            Vector3 parentPosition = new Vector2(-6.5f, 1f); ;
            parent.transform.position = parentPosition;
            Instantiate(agent, parentPosition, Quaternion.identity);

            // Set the parent for agent
            agent.transform.parent = parent.transform;

            Seek();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Instantiate targetSprite
            positionT = new Vector2(6f, 1f);
            Instantiate(target, positionT, Quaternion.identity);

            // Parent
            GameObject parent = GameObject.Find("Parent");

            // Instantiate agentSprite
            Vector3 parentPosition = new Vector2(5f, 1f); ;
            parent.transform.position = parentPosition;
            Instantiate(agent, parentPosition, Quaternion.identity);

            // Set the parent for agent
            agent.transform.parent = parent.transform;
            // Flee();
            Flee();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Instantiate targetSprite
            positionT = new Vector2(6f, 1f);
            Instantiate(target, positionT, Quaternion.identity);

            // Parent
            GameObject parent = GameObject.Find("Parent");

            // Instantiate agentSprite
            Vector3 parentPosition = new Vector2(-6.5f, -2.5f); ;
            parent.transform.position = parentPosition;
            Instantiate(agent, parentPosition, Quaternion.identity);

            // Set the parent for agent
            agent.transform.parent = parent.transform;

            Arrive();
        }
        else if(!Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Instantiate targetSprite
            positionT = new Vector2(6f, 1f);
            Instantiate(target, positionT, Quaternion.identity);

            // Instantiate obstacle
            positionO = new Vector2(0f, 1f);
            Instantiate(obstacle, positionO, Quaternion.identity);

            // Parent
            GameObject parent = GameObject.Find("Parent");

            // Instantiate agentSprite
            Vector3 parentPosition = new Vector2(-6.5f, -2.5f); ;
            parent.transform.position = parentPosition;
            Instantiate(agent, parentPosition, Quaternion.identity);

            // Set the parent for agent
            agent.transform.parent = parent.transform;
            
            Seek();
            AvoidObstacles();
        }
        else if(!Input.GetKeyDown(KeyCode.Alpha5))
        {
            Destroy(obstacle);
            Destroy(agent);
            Destroy(target);
        }
    }

    // Cast whiskers to detect obstacles.
    private bool CastWhisker(float angle)
    {

        Color rayColor = Color.red;
        bool hitResult = false;

        

        // Calculate the direction of the whisker
        Vector2 whiskerDirection = Quaternion.Euler(0, 0, angle) * transform.up;

        // Cast a ray in the whisker direction;
        RaycastHit2D hit = Physics2D.Raycast(agent.transform.position, whiskerDirection, whiskerLength);

        // Check if the ray hits an obstacle
        if (hit.collider != null)
        {
            Debug.Log("Obstacle Detected!!!");

            rayColor = Color.green;
            hitResult = true;
        }
        Debug.DrawRay(agent.transform.position, whiskerDirection * whiskerLength, rayColor);
        return hitResult;
    }

    private void Seek()
{
    // Update positions based on GameObjects' actual positions
    positionA = transform.position;
    positionT = GameObject.Find("Target(Clone)").transform.position;
    // Calculate direction to the target.
    Vector2 direction = (target.transform.position - agent.transform.position).normalized;
    // Calculate the Velocity
    Vector2 velocity = direction * movementSpeed;
    // Set velocity
    GetComponent<Rigidbody>().velocity = velocity;

    // Calculate the angle to rotate towards the target.
    //float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90.0f;

    //// Smoothly rotate towards the target.
    //float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
    //float rotationStep = rotationSpeed * Time.deltaTime;
    //float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
    //transform.Rotate(Vector3.forward, rotationAmount);

    //// Move directly towards the target using Rigidbody2D.
    //rb.velocity = directionToTarget * movementSpeed;
}
    private void Flee()
    {
        // Calculate direction to the target.
        Vector2 direction = (agent.transform.position - target.transform.position).normalized;
        // Calculate the Velocity
        Vector2 velocity = direction * movementSpeed;
        // Set velocity
        GetComponent<Rigidbody>().velocity = velocity;
    }

    private void Arrive()
    {
        // Calculate direction to the target.
        Vector2 direction = (target.transform.position - agent.transform.position).normalized;

        float distance = direction.magnitude;
        if (distance < 5f)
        {
            // Calculate the Velocity
            Vector2 velocity = direction.normalized * movementSpeed * (distance / 5f);
            // Set velocity
            GetComponent<Rigidbody>().velocity = velocity;
        }
        else
        {
            // Calculate the Velocity
            Vector2 velocity = direction * movementSpeed;
            // Set velocity
            GetComponent<Rigidbody>().velocity = velocity;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Target")
        {
            GetComponent<AudioSource>().Play();
            // What is this!?! Didn't you learn how to create a static sound manager last week in 1017?
        }
    }
}
