using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public GameObject agentSprite;
    public GameObject targetSprite;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // Spawn Agent
            Vector3 spawnPositionA = new Vector3(-6.5f, -2.5f, 0);
            Instantiate(agentSprite, spawnPositionA, Quaternion.identity);

            // Spawn Target
            Vector3 spawnPositionT = new Vector3(6f, 1f, 0);
            Instantiate(targetSprite, spawnPositionT, Quaternion.identity);


        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Spawn Agent
            Vector3 spawnPositionA = new Vector3(-6.5f, -2.5f, 0);
            Instantiate(agentSprite, spawnPositionA, Quaternion.identity);

            // Spawn Target
            Vector3 spawnPositionT = new Vector3(6f, 1f, 0);
            Instantiate(targetSprite, spawnPositionT, Quaternion.identity);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
        else if (!Input.GetKeyDown(KeyCode.Alpha4))
        { 

        }
        else if (!Input.GetKeyDown(KeyCode.Alpha5))
        {

        }
    }
}
