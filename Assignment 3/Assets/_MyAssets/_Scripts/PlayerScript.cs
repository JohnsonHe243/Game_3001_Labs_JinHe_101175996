using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float rotSpeed;
    [SerializeField] float moveForce;
    [SerializeField] float maxSpeed;
    [SerializeField] float magnitude;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Rotate the ship
        transform.Rotate(new Vector3(0f, 0f, Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime));


        // Add forward thrust.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector2 force = transform.right * moveForce * Time.deltaTime;
            rb.AddForce(force);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector2 force = transform.right * moveForce * Time.deltaTime;
            rb.AddForce(-force);
        }
    }

    void FixedUpdate()
    {
        // Clamp the magnitude of the ship.
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        // Print the magnitude of the ship.
        magnitude = rb.velocity.magnitude;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
