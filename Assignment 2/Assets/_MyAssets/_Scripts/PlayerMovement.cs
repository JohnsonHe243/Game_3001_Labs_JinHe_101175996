using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float stepSize = 1f; // Adjust this to change the step size

    void Update()
    {

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
        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
        }

        // Move

        if (Input.GetKeyUp(KeyCode.W))
        {
            transform.Translate(Vector3.up * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player down when the S key is pressed
        if (Input.GetKeyUp(KeyCode.S))
        {
            transform.Translate(Vector3.down * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }

        // Move the player left when the A key is pressed
        if (Input.GetKeyUp(KeyCode.A))
        {
            transform.Translate(Vector3.left * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);

        }

        // Move the player right when the D key is pressed
        if (Input.GetKeyUp(KeyCode.D))
        {
            transform.Translate(Vector3.right * stepSize);
            GetComponent<NavigationObject>().SetGridIndex();
            Vector2 tileIndex = GetComponent<NavigationObject>().GetGridIndex();
            GridManager.Instance.GetGrid()[(int)tileIndex.y, (int)tileIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }
    }


}
