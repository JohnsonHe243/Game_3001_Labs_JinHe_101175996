using System.Collections;
using System.Collections.Generic;
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
    public float Cost { get; set; } //This is a new cost from tile to tile. We'll use distance in units

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
    public PathConnection PathConnection { get; set; }
    public float CostSoFar { get; set; }

    public NodeRecord(PathNode node = null)
    {
        Node = node;
        PathConnection = null;
        FromRecord = null;
        CostSoFar = 0f;
    }
}

public class PathManager : MonoBehaviour
{
    public List<NodeRecord> openList;
    public List<NodeRecord> closeList;

    public List<PathConnection> path; //What will be the shortest path

    public static PathManager Instance { get; private set; } // Static object of the class

    private void Awake()
    {
        if (Instance == null) // If the object/instance doesn't exist yet
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject); //Destroy dublicate instances
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
        //TODO
    }

    //Some utility methods

    public NodeRecord GetsmallestNode()
    {
        NodeRecord smallestNode = openList[0];

        //Iterate through the rest f the noderecords in the list
        for (int i = 1; i < openList.Count; i++)
        {
            //If the current NodeRecord has a smaller CostSofar than the smallestNode, update smallestNode with Current NodeRecord
            if (openList[i].CostSoFar < smallestNode.CostSoFar)
            {
                smallestNode = openList[i];
            }
            // If they're the same, flip a coin
            else if (openList[i].CostSoFar == smallestNode.CostSoFar)
            {
                smallestNode = (Random.value < 0.5 ? openList[i] : smallestNode);
            }
        }

        return smallestNode; //Return the nodeRecord with the smallest CostSoFar
    }

    public bool ContainsNode(List<NodeRecord> list, PathNode node)
    {
        foreach (NodeRecord record in list)
        {
            if (record.Node == node) return true;
        }

        return false;
    }

    public NodeRecord GetNodeRecord(List<NodeRecord> list, PathNode node)
    {
        foreach (NodeRecord record in list)
        {
            if (record.Node == node) return record;
        }
        return null;
    }
}
