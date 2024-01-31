using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnTargets : MonoBehaviour
{
    public GameObject targetSprite;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Spawn Target
            Vector3 spawnPositionT = new Vector3(6f, 1f, 0);
            Instantiate(targetSprite, spawnPositionT, Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
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
