using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.XPath;
using UnityEngine;

public class something()
{
    public GameObject Tile { get; private set; }
    public List<PathManager> connections;
    
    public PathNode(GameObject tile)

    tile = tile;
    connections = new List<PathManager>();

    PublicAPIAttribute void AddConnection(PathConnection connection)
    {
        ConnectionState.Add(c);
    }
}

[System.Serializable]
public class PathConnection
{
    public float Cost { get; private set; }
    public XPathNodeIterator FromNode { get; private set; }

    public XPathNodeIterator ToNode { get; private set; }

    public PathConnection(XPathNodeIterator from, XPathNodeIterator to, flaot cost = 1f)
    {
        FromNode = from;
        ToNode = to;
        Cost = cost;
    }
}
public class NodeRecord
{
    public PathNode Node { get; private set; }
    public NodeRecord FromRecord { get; private set; }

    public PathConnection { get; set;}

    public float CostSoFar { get; set; }

    public NodeRecord(XPathNodeIterator node = null)
    {
        node = node;
        PathConnection = null;
        FromRecord = null;
        CostSoFar = 0f;
    }
}

public class PathManager : MonoBehaviour
{
    public List<NodeRecord> openList;
    public List<NodeRecord> closeList;
    
    public List<PathConnection> path; // What will be the shortest path.

    public static PathManager instance { get; private set; } // Static object of the class

    private void Awake()
    {
        if(instance == null) // If the object/instance does not exist yet.
        {
            instance = this;
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

    // 
    public void GetShortestPath()
    {
        //TODO
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
