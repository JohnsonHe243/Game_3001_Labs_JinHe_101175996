using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float stepSize = 1f; // Adjust this to change the step size
    [SerializeField] float whiskerLength = .5f;
    [SerializeField] float upWhiskerAngle;
    [SerializeField] float leftWhiskerAngle;
    [SerializeField] float downWhiskerAngle;
    [SerializeField] float rightWhiskerAngle;

    void Update()
    {
        bool hitUp = CastWhisker(upWhiskerAngle);
        bool hitLeft = CastWhisker(leftWhiskerAngle);
        bool hitDown = CastWhisker(downWhiskerAngle);
        bool hitRight = CastWhisker(rightWhiskerAngle);

        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
            TileScript currentTile = GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>();

        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
            TileScript currentTile = GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>();
        }

        // Move

        if (Input.GetKeyUp(KeyCode.W) && hitUp == false)
        {
            transform.Translate(Vector3.up * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player down when the S key is pressed
        if (Input.GetKeyUp(KeyCode.S) && hitDown == false)
        {
            transform.Translate(Vector3.down * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player left when the A key is pressed
        if (Input.GetKeyUp(KeyCode.A) && hitLeft == false)
        {
            transform.Translate(Vector3.left * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player right when the D key is pressed
        if (Input.GetKeyUp(KeyCode.D) && hitRight == false)
        {
            transform.Translate(Vector3.right * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
            TileScript currentTile = GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>();
        }
    }

    private bool CastWhisker(float angle)
    {
        Color rayColor = Color.red;
        bool hitResult = false;


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

}
