using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathNode
{
    public GameObject Tile { get; private set; }
    public List<PathConnection> connections;

    public PathNode(GameObject tile)
    {
        Tile = tile;
        connections = new List<PathConnection>();
    }
    public void AddConnection(PathConnection c)
    {
        connections.Add(c);
    }
}

[System.Serializable]
public class PathConnection
{
    public float Cost { get; set; }
    public PathNode FromNode { get; private set; }

    public PathNode ToNode { get; private set; }

    public PathConnection(PathNode from, PathNode to, float cost = 1f)
    {
        FromNode = from;
        ToNode = to;
        Cost = cost;
    }
}
public class NodeRecord
{
    public PathNode Node { get; set; }
    public NodeRecord FromRecord { get; set; }

    public PathConnection PathConnection { get; set;}

    public float CostSoFar { get; set; }

    public NodeRecord(PathNode node = null)
    {
        Node = node;
        PathConnection = null;
        FromRecord = null;
        CostSoFar = 0f;
    }
}
[System.Serializable]

public class PathManager : MonoBehaviour
{
    public List<NodeRecord> openList;
    public List<NodeRecord> closeList;
    public List<PathConnection> path; // What will be the shortest path.

    public static PathManager Instance { get; private set; } // Static object of the class

    private void Awake()
    {
        if(Instance == null) // If the object/instance does not exist yet.
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject); // Destroy dublicate instances
        }
    }

    private void Initialize()
    {
        openList = new List<NodeRecord>();
        closeList = new List<NodeRecord>();
        path = new List<PathConnection>();
    }

    public void GetShortestPath(PathNode start, PathNode goal)
    {
        if(path.Count >0)
        {
            GridManager.Instance.SetTileStatuses();
            path.Clear(); // Clear Previous Path
        }

        NodeRecord currentRecord = null;
        openList.Add(new NodeRecord(start));
        while(openList.Count > 0)
        {
            currentRecord = GetSmallestNode();
            if (currentRecord.Node == goal)
            {
                // We found the goal!
                openList.Remove(currentRecord);
                closeList.Add(currentRecord);
                currentRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.CLOSED);
                break;
            }
            List<PathConnection> connections = currentRecord.Node.connections;
            for(int i = 0; i < connections.Count; i++)
            {
                PathNode endNode = connections[i].ToNode;
                NodeRecord endNodeRecord;
                float endNodeCost = currentRecord.CostSoFar + connections[i].Cost;

                if (ContainsNode(closeList, endNode)) continue;
                else if (ContainsNode(openList, endNode))
                {
                    endNodeRecord = GetNodeRecord(openList, endNode);
                    if (endNodeRecord.CostSoFar <= endNodeCost)
                        continue;
                }
                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.Node = endNode;
                }
                endNodeRecord.CostSoFar = endNodeCost;
                endNodeRecord.PathConnection = connections[i];
                endNodeRecord.FromRecord = currentRecord;
                if(!ContainsNode(openList, endNode))
                {
                    openList.Add(endNodeRecord);
                    endNodeRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.CLOSED);
                }
            }
            openList.Remove(currentRecord);
            closeList.Add(currentRecord);
            currentRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.CLOSED);
        }
        if (currentRecord == null) return;
        if(currentRecord.Node != goal)
        {
            Debug.Log("Could not find path to goal");
        }
        else
        {
            Debug.Log("Found path to goal!");
            while (currentRecord.Node != start) 
            {
                path.Add(currentRecord.PathConnection);
                currentRecord.Node.Tile.GetComponent<TileScript>().SetStatus(TileStatus.PATH);
                currentRecord = currentRecord.FromRecord;
            }
            path.Reverse();
        }
        openList.Clear();
        closeList.Clear();

    }

    // Some utility methods

    public NodeRecord GetSmallestNode()
    {
        NodeRecord smallestNode = openList[0];

        //iterate through the rest of the node records in the list
        for(int i = 1; i < openList.Count; i++)
        {
            // If the current NodeRecord has a smaller CostSoFar than the smallestNode, update smallestNode with current NodeRecord.
            if (openList[i].CostSoFar < smallestNode.CostSoFar)
            {
                smallestNode = openList[i];
            }
            // If they are the same flip a coin.
            else if (openList[i].CostSoFar == smallestNode.CostSoFar)
            {
                smallestNode = (Random.value < 0.5 ? openList[i] : smallestNode);
            }
        }

        return smallestNode; // Return the NodeRecord with the smallest CostSoFar
    }

    public bool ContainsNode(List<NodeRecord> list, PathNode node)
    {
        foreach(NodeRecord record in list)
        {
            if (record.Node == node) return true;
        }
        return false;
    }

    public NodeRecord GetNodeRecord(List<NodeRecord> list, PathNode node)
    {
        foreach (NodeRecord record in list)
        {
            if(record.Node == node) return record;
        }
        return null;
    }
}
