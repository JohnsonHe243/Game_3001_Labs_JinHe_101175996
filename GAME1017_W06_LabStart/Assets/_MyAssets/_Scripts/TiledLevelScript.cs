using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TiledLevelScript : MonoBehaviour
{

    [SerializeField] private Tilemap[] tileMaps;
    [SerializeField] private TileBase[] tileBases;
    [SerializeField] private char[] tileKeys;
    [SerializeField] private char[] tileObstacles;
    private int rows = 24; // Y-axix
    private int cols = 32; // X-axis
    // Start is called before the first frame update
    void Start()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        try
        {
            // Load tile data first.
            using(StreamReader reader = new StreamReader("Assets/TileData.txt"))
            {
                // Read all tile chars and create an array from it.
                string line = reader.ReadLine();
                tileKeys = line.ToCharArray();
                // Next is the obstacle tiles
                line = reader.ReadLine();
                tileObstacles = line.ToCharArray();
                // We can also do the hazards. Next time.
            }
            // Then load level data.
            using (StreamReader reader = new StreamReader("Assets/Level1.txt"))
            {
                string line;
                for (int row  = 1; row < rows + 1; row++)
                {
                    line = reader.ReadLine();
                    for (int col = 0; col < cols + 1; col++)
                    {
                        char c = line[col];
                        if (c == '*') continue;

                        int charIndex = Array.IndexOf(tileKeys, c);
                        if ((charIndex == -1) throw new Exception("Index not found.");
                        // Check if tile is obstacle or normal.
                        if (Array.IndexOf(tileObstacles, c) > -1) // Tile is obstacl
                        {
                            SetTile(0, charIndex, col, row);
                        }
                        else // Tile is normal.
                        {
                            SetTile(1, charIndex, col, row);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void SetTile(int TileMapIndex, int charIndex, int col, int row)
    {
        // Check all tilemaps to see if there's a manually-painted tile there.
        foreach (Tilemap tilemap in tileMaps)
        {
            if(tilemap.HasTile(new Vector3Int(col, row, 0))) return;
        }
        // if no tile, then set the tile in the desired tilemap.
        tileMaps[TileMapIndex].SetTile(new Vector3Int(col, -row, 0), tileBases[charIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
