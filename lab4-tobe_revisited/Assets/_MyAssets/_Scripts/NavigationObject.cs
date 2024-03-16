using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationObject : MonoBehaviour
{
    public Vector2 gridIndex;
    // Start is called before the first frame update

    private void Awake()
    {
        gridIndex = new Vector2();
        SetGridIndex();
    }

    public Vector2 GetGridIndex()
    {
        return gridIndex;
    }
    public void SetGridIndex()
    {
        float originalX = Mathf.Floor(transform.position.x) + 0.5f;
        gridIndex.x = ((int)Mathf.Floor(originalX + 7.5f) / 15 * 15);
        float originalY = Mathf.Floor(transform.position.y) + 0.5f;
        gridIndex.y = ((int)Mathf.Floor(originalY + 5.5f));

        //float xPos = Mathf.Floor(worldPosition.x) + 0.5f;
        //float yPos = Mathf.Floor(worldPosition.y) + 0.5f;
        //return new Vector2(xPos, yPos);
    }
}
