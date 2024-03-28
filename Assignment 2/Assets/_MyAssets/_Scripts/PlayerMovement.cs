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
        
        // Change Tile Position when Movement or reset k

        if (Input.GetKeyDown(KeyCode.W))
        {
            GridManager.Instance.ResetStartTiles();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GridManager.Instance.ResetStartTiles();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GridManager.Instance.ResetStartTiles();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            GridManager.Instance.ResetStartTiles();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GridManager.Instance.ResetStartTiles();
            GridManager.Instance.ResetGoalTiles();
        }
        // Move

        if (Input.GetKeyUp(KeyCode.W) && hitUp == false)
        {
            transform.Translate(Vector3.up * stepSize);
            //GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player down when the S key is pressed
        if (Input.GetKeyUp(KeyCode.S) && hitDown == false)
        {
            transform.Translate(Vector3.down * stepSize);
            //GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player left when the A key is pressed
        if (Input.GetKeyUp(KeyCode.A) && hitLeft == false)
        {
            transform.Translate(Vector3.left * stepSize);
            //GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player right when the D key is pressed
        if (Input.GetKeyUp(KeyCode.D) && hitRight == false)
        {
            transform.Translate(Vector3.right * stepSize);
            //GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Moves Player and set tile to player position
            transform.position = new Vector3(-7.5f, 5.5f, 0f);
            //GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);

            // Reset goal mushroom tile
            GameObject mush = GameObject.FindGameObjectWithTag("Mushroom");
            //mush.GetComponent<NavigationObject>().SetGridIndex();
            tileIndex = mush.GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
        }
        // Move along the path when pressed M
        //if (Input.GetKeyUp(KeyCode.M)) // Reset start tile to CLOSED
        //{
        //    GridManager.Instance.ResetStartTiles();
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
        //    GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].status;
        //}

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
