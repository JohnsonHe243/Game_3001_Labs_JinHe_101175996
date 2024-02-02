using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject blue;
    public GameObject mushroom;
    public GameObject apple;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Instantiate apple
            Instantiate(apple, new Vector2(4f, -2.5f), Quaternion.identity);

            // Instantiate blue
            Instantiate(blue, new Vector2(-10f, 5), Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Instantiate apple
            Instantiate(apple);

            // Instantiate blue
            Instantiate(blue, new Vector2(5f, -2.5f), Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Instantiate apple
            Instantiate(apple);

            // Instantiate blue
            Instantiate(blue, new Vector2(-10f, 6f), Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Instantiate apple
            Instantiate(apple);
            // Instantiate mushroom
            Instantiate(mushroom, new Vector2(0f, -2.5f), Quaternion.identity);

            // Instantiate blue
            Instantiate(blue, new Vector2(-10f, -2f), Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) // remove from scene 
        {
            GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("destroy");
            foreach (GameObject obj in objectsToDelete)
            {
                Destroy(obj);
            }
        }
    }
}
