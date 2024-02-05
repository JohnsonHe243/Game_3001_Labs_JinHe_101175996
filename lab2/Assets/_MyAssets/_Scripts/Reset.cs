using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public GameObject reliant;
    public GameObject planet;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in objectsToDelete)
            {
                Destroy(obj);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Instantiate(reliant);  
            Instantiate(planet);
        }
    }
}
