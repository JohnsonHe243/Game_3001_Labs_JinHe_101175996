using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BlueArrive : AgentObject

{
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float rotationSpeed = 60f;
    // Add fields for whisper length, angle and avoidance weight.
    [SerializeField] float slowRadius = 1f;
    private Rigidbody2D rb;

    new void Start() // Note the new.
    {
        base.Start(); // Explicitly invoking Start of AgentObject.
        Debug.Log("Starting...");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (TargetPosition != null)
        {
            Arrive();
        }
    }

    private void Arrive() // A seek with rotation to target but only moving along forward vector.
    {
        // Calculate direction to the target.
        Vector2 directionToTarget = (TargetPosition - transform.position).normalized;

        // Calculate the angle to rotate towards the target.
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + 90.0f; // Note the +90 when converting from Radians.

        // Smoothly rotate towards the target.
        float angleDifference = Mathf.DeltaAngle(targetAngle, transform.eulerAngles.z);
        float rotationStep = rotationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);

        // Distance
        float distance = (TargetPosition - transform.position).magnitude;


        // Move along the forward vector using Rigidbody2D.\
        if (distance < slowRadius) 
        {
            transform.Rotate(Vector3.forward, rotationAmount * (distance/slowRadius));
            rb.velocity = transform.up * movementSpeed * (distance / slowRadius);
        }
        else
        {
            transform.Rotate(Vector3.forward, rotationAmount);
            rb.velocity = transform.up * movementSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Target")
        {
            GetComponent<AudioSource>().Play();
        }
    }
}