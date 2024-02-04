using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject blueSeek;
    public GameObject blueFlee;
    public GameObject blueArrive;
    public GameObject blueSeekAvoid;
    public GameObject mushroom;
    public GameObject mushroomSmall;
    public GameObject apple;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Instantiate apple
            Instantiate(apple);

            // Instantiate blue
            Instantiate(blueSeek);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Instantiate apple
            Instantiate(mushroomSmall);

            // Instantiate blue
            Instantiate(blueFlee);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Instantiate apple
            Instantiate(apple);

            // Instantiate blue
            Instantiate(blueArrive);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Instantiate apple
            Instantiate(apple);
            // Instantiate mushroom
            Instantiate(mushroom);

            // Instantiate blue
            Instantiate(blueSeekAvoid);

        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) // remove from scene 
        {   // Created multiple tags bc sound effects
            GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("destroy");
            foreach (GameObject obj in objectsToDelete)
            {
                Destroy(obj);
            }
            GameObject[] objectsToDeleteT = GameObject.FindGameObjectsWithTag("Target");
            foreach (GameObject obj in objectsToDeleteT)
            {
                Destroy(obj);
            }
            GameObject[] objectsToDeleteO = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject obj in objectsToDeleteO)
            {
                Destroy(obj);
            }
        }
    }
}
