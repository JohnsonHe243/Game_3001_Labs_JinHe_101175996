using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour

{
    public GameObject agentSprite;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Spawn Agent
            Vector3 spawnPositionA = new Vector3(-6.5f, -2.5f, 0);
            Instantiate(agentSprite, spawnPositionA, Quaternion.identity);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Spawn Agent
            Vector3 spawnPositionA = new Vector3(-6.5f, -2.5f, 0);
            Instantiate(agentSprite, spawnPositionA, Quaternion.identity);
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
