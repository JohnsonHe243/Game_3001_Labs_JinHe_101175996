using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;

public enum TileStatus
{
    UNVISITED,
    OPEN,
    CLOSED,
    IMPASSABLE,
    GOAL,
    START,
    PATH
};

public enum NeighbourTile
{
    TOP_TILE,
    RIGHT_TILE,
    BOTTOM_TILE,
    LEFT_TILE,
    NUM_OF_NEIGHBOUR_TILES
};

public class GridManager : MonoBehaviour
{
    private List<PathConnection> path = new List<PathConnection>();
    private int currentPathIndex = 0; // Track the index of the current tile in the path
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tilePanelPrefab;
    [SerializeField] private GameObject panelParent;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private Color[] colors;
    [SerializeField] private float baseTileCost = 1f;
    [SerializeField] private bool useManhattanHeuristic = true;

    private GameObject[,] grid;
    private int rows = 10;
    private int columns = 10;
    private List<GameObject> mines = new List<GameObject>();
    private bool setActive = true;

    public static GridManager Instance { get; private set; } // Static object of the class.

    void Awake()
    {
        if (Instance == null) // If the object/instance doesn't exist yet.
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Initialize()
    {
        BuildGrid();
        ConnectGrid();
    }

    private void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        panelParent.gameObject.SetActive(!panelParent.gameObject.activeSelf);
        setActive = false;

        GameObject[] Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in Obstacles)
        {
            Vector2 appleIndex = obstacle.GetComponent<NavigationObject>().GetGridIndex();
            grid[(int)appleIndex.y, (int)appleIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.IMPASSABLE);
            ConnectGrid();
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            panelParent.gameObject.SetActive(!panelParent.gameObject.activeSelf);
            setActive = !setActive;
        }

        if (Input.GetKeyDown(KeyCode.F)) // Start path finding. 
        {

            // Find start tile
            Vector2 startIndices = FindStartTile();
            PathNode start = grid[(int)startIndices.y, (int)(startIndices.x)].GetComponent<TileScript>().Node;

            // Get Goal Node
            Vector2 goalIndices = FindGoalTile();
            PathNode goal = grid[(int)goalIndices.y, (int)goalIndices.x].GetComponent<TileScript>().Node;

            // Start Algorithm
            PathManager.Instance.GetShortestPath(start, goal);

            GameObject[] Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject obstacle in Obstacles)
            {
                Vector2 appleIndex = obstacle.GetComponent<NavigationObject>().GetGridIndex();
                grid[(int)appleIndex.y, (int)appleIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.IMPASSABLE);
            }
            // Keep Status
            //grid[(int)startIndices.y, (int)(startIndices.x)].GetComponent<TileScript>().SetStatus(TileStatus.START);
            //grid[(int)goalIndices.y, (int)goalIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
        }

        if (setActive == true)
        {
            Vector2 originalM;
            Vector2 originalP;
            Vector2 mouseIndexRight;
            Vector2 mouseIndexLeft;

            if (Input.GetMouseButtonDown(0))
            {
                // Set Player tile to unvisted
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                originalP = player.GetComponent<NavigationObject>().GetGridIndex();
                grid[(int)originalP.y, (int)originalP.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
                ResetStartTiles();
            }
            if (Input.GetMouseButtonUp(0))
            {
                // Set Clicked tile to start
                Vector2 mouse = GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float originalX = Mathf.Floor(mouse.x) + 0.5f;
                mouseIndexLeft.x = (int)Mathf.Floor((originalX + 7.5f) / 15 * 15);
                float originalY = Mathf.Floor(mouse.y) + 0.5f;
                mouseIndexLeft.y = 11 - (int)Mathf.Floor(originalY + 5.5f);
                grid[(int)mouseIndexLeft.y, (int)mouseIndexLeft.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
            }
            if (Input.GetMouseButtonDown(1))
            {
                // Set Mushroom tile to unvisited
                GameObject mushroom = GameObject.FindGameObjectWithTag("Mushroom");
                originalM = mushroom.GetComponent<NavigationObject>().GetGridIndex();
                grid[(int)originalM.y, (int)originalM.x].GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
                ResetGoalTiles();

            }
            if (Input.GetMouseButtonUp(1))
            {
                // Set Clicked tile to goal
                Vector2 mouse = GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                float originalX = Mathf.Floor(mouse.x) + 0.5f;
                mouseIndexRight.x = (int)Mathf.Floor((originalX + 7.5f) / 15 * 15);
                float originalY = Mathf.Floor(mouse.y) + 0.5f;
                mouseIndexRight.y = 11 - (int)Mathf.Floor(originalY + 5.5f);
                grid[(int)mouseIndexRight.y, (int)mouseIndexRight.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
                originalM = mouseIndexRight;
                SetTileCosts(originalM);
            }


        }
 
    }

    private void BuildGrid()
    {
        grid = new GameObject[rows, columns];
        int count = 0;
        float rowPos = 5.5f;
        for (int row = 0; row < rows; row++, rowPos--)
        {
            float colPos = -7.5f;
            for (int col = 0; col < columns; col++, colPos++)
            {
                GameObject tileInst = GameObject.Instantiate(tilePrefab, new Vector3(colPos, rowPos, 0f), Quaternion.identity);
                TileScript tileScript = tileInst.GetComponent<TileScript>();
                tileScript.SetColor(colors[System.Convert.ToInt32((count++ % 2 == 0))]);

                tileInst.transform.parent = transform;
                grid[row, col] = tileInst;

                // Instantiate a new TilePanel and link it to the Tile instance.
                GameObject panelInst = GameObject.Instantiate(tilePanelPrefab, tilePanelPrefab.transform.position, Quaternion.identity);
                panelInst.transform.SetParent(panelParent.transform);
                RectTransform panelTransform = panelInst.GetComponent<RectTransform>();
                panelTransform.localScale = Vector3.one;
                panelTransform.anchoredPosition = new Vector3(64f * col, -64f * row);
                tileScript.tilePanel = panelInst.GetComponent<TilePanelScript>();
                // Create a new PathNode for the new tile
                tileScript.Node = new PathNode(tileInst);
            }
            count--;
        }
        // Set the tile under the ship to Start.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerIndices = player.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)playerIndices.y, (int)playerIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        // Set the tile under the player to Goal and set tile costs.
        GameObject mush = GameObject.FindGameObjectWithTag("Mushroom");
        Vector2 mushIndices = mush.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)mushIndices.y, (int)mushIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
        SetTileCosts(mushIndices);
    }

    public void ConnectGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                TileScript tileScript = grid[row, col].GetComponent<TileScript>();
                tileScript.ResetNeighbourConnections();
                if (tileScript.status == TileStatus.IMPASSABLE) continue;
                if (row > 0) // Set top neighbour if tile is not in top row.
                {
                    if (!(grid[row - 1, col].GetComponent<TileScript>().status == TileStatus.IMPASSABLE))
                    {
                        tileScript.SetNeighbourTile((int)NeighbourTile.TOP_TILE, grid[row - 1, col]);
                        tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row - 1, col].GetComponent<TileScript>().Node,
                            Vector3.Distance(tileScript.transform.position, grid[row - 1, col].transform.position)));
                    }

                }
                if (col < columns - 1) // Set right neighbour if tile is not in rightmost row.
                {
                    if (!(grid[row, col + 1].GetComponent<TileScript>().status == TileStatus.IMPASSABLE))
                    {
                        tileScript.SetNeighbourTile((int)NeighbourTile.RIGHT_TILE, grid[row, col + 1]);
                        tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row, col + 1].GetComponent<TileScript>().Node,
                            Vector3.Distance(tileScript.transform.position, grid[row, col + 1].transform.position)));
                    }

                }
                if (row < rows - 1) // Set bottom neighbour if tile is not in bottom row.
                {
                    if (!(grid[row + 1, col].GetComponent<TileScript>().status == TileStatus.IMPASSABLE))
                    {
                        tileScript.SetNeighbourTile((int)NeighbourTile.BOTTOM_TILE, grid[row + 1, col]);
                        tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row + 1, col].GetComponent<TileScript>().Node,
                            Vector3.Distance(tileScript.transform.position, grid[row + 1, col].transform.position)));
                    }
                }
                if (col > 0) // Set left neighbour if tile is not in leftmost row.
                {
                    if (!(grid[row, col - 1].GetComponent<TileScript>().status == TileStatus.IMPASSABLE))
                    {
                        tileScript.SetNeighbourTile((int)NeighbourTile.LEFT_TILE, grid[row, col - 1]);
                        tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row, col - 1].GetComponent<TileScript>().Node,
                            Vector3.Distance(tileScript.transform.position, grid[row, col - 1].transform.position)));
                    }
                }
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return grid;
    }
    public Vector2 GetGridPosition(Vector2 worldPosition)
    {
        float xPos = Mathf.Floor(worldPosition.x) + 0.5f;
        float yPos = Mathf.Floor(worldPosition.y) + 0.5f;
        return new Vector2(xPos, yPos);
    }

    public void SetTileCosts(Vector2 targetIndices)
    {
        float distance = 0f;
        float dx = 0f;
        float dy = 0f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                TileScript tileScript = grid[row, col].GetComponent<TileScript>();
                if (useManhattanHeuristic)
                {
                    dx = Mathf.Abs(col - targetIndices.x);
                    dy = Mathf.Abs(row - targetIndices.y);
                    distance = dx + dy;
                }
                else // Euclidean.
                {
                    dx = targetIndices.x - col;
                    dy = targetIndices.y - row;
                    distance = Mathf.Sqrt(dx * dx + dy * dy);
                }
                float adjustedCost = distance * baseTileCost;
                tileScript.cost = adjustedCost;
                tileScript.tilePanel.costText.text = tileScript.cost.ToString("F1");
                
                // ToDo: calculate the total cost of path
                if (tileScript.status == TileStatus.PATH) // Check if the tile status is PATH.
                {
                    // Calculate cost based on different heuristics.
                    if (useManhattanHeuristic)
                    {
                        dx = Mathf.Abs(col - targetIndices.x);
                        dy = Mathf.Abs(row - targetIndices.y);
                        distance = dx + dy;
                    }
                    else // Euclidean.
                    {
                        dx = targetIndices.x - col;
                        dy = targetIndices.y - row;
                        distance = Mathf.Sqrt(dx * dx + dy * dy);
                    }
                    float pathCost = +adjustedCost;

                }
            }
        }
    }
    public void SetTileStatuses()
    {
        foreach (GameObject go in grid)
        {
            go.GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
        }
        foreach (GameObject mine in mines)
        {
            Vector2 mineIndex = mine.GetComponent<NavigationObject>().GetGridIndex();
            grid[(int)mineIndex.y, (int)mineIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.IMPASSABLE);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerIndices = player.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)playerIndices.y, (int)playerIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.START);

        GameObject mush = GameObject.FindGameObjectWithTag("Mushroom");
        Vector2 mushIndices = mush.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)mushIndices.y, (int)mushIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
    }

    public void ResetStartTiles()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                TileScript tileScript = grid[i, j].GetComponent<TileScript>();
                if (tileScript.status == TileStatus.START)
                {
                    grid[i, j].GetComponent<TileScript>().SetStatus(TileStatus.CLOSED);
                }
            }
        }
    }

    public void ResetGoalTiles()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                TileScript tileScript = grid[i, j].GetComponent<TileScript>();
                if (tileScript.status == TileStatus.GOAL)
                {
                    grid[i, j].GetComponent<TileScript>().SetStatus(TileStatus.CLOSED);
                }
            }
        }
    }

    Vector2Int FindStartTile()
{
    for (int i = 0; i < grid.GetLength(0); i++)
    {
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            TileScript tileScript = grid[i, j].GetComponent<TileScript>();
            if (tileScript.status == TileStatus.START)
            {
                return new Vector2Int(j, i); // Return the indices (x, y) of the start tile
            }
        }
    }
    return new Vector2Int(-1, -1); // Return (-1, -1) if start tile not found
}

Vector2Int FindGoalTile()
{
    for (int i = 0; i < grid.GetLength(0); i++)
    {
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            TileScript tileScript = grid[i, j].GetComponent<TileScript>();
            if (tileScript.status == TileStatus.GOAL)
            {
                return new Vector2Int(j, i); // Return the indices (x, y) of the goal tile
            }
        }
    }
    return new Vector2Int(-1, -1); // Return (-1, -1) if goal tile not found
}
}


