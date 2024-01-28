using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShip : AgentObject
{
    // Start is called before the first frame update
    [SerializeField] float movementSpeed = 1.0f;

    [SerializeField] float rotationSpeed = 155.0f;    
    Rigidbody2D rb;


    new void Start() // Note the new!
    {
        base.Start(); // Explicitly invoking start of AgentObject
        Debug.Log("Starting Agent");

        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame

    void Update()
    {
       if (TargetPosition != null)
        {
            //Seek();
            SeekForward();
        } 
    }

    private void Seek()
    {
        // Calculate desired velocity using kinematic see equation.
        Vector2 desiredVelocity = (TargetPosition - transform.position).normalized * movementSpeed;

        // Calculate the steering force.
        Vector2 steeringForce = desiredVelocity - rb.velocity;

        //Apply the steering force to the agent.
        rb.AddForce(steeringForce);
    }

    private void SeekForward() // Always move toward while rotate to the target.
    {
        // Calculate direction to the target.
        Vector2 directionToTarget = (TargetPosition - transform.position).normalized;

        // Calculate the angle to rotate towards the target.
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.y) * Mathf.Rad2Deg+90.0f;

        // Smoothly rotate towards the target.
        float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z); // Delta angle from target to ship.
        float rotationStep = rotationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);

        transform.Rotate(Vector3.forward, rotationAmount);

        // Move along the forward vector using Rigidbody2D.
        rb.velocity = transform.up * movementSpeed;
    }
}