using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections = new List<Node>();

    public float gScore;
    public float hScore;

    public float FScore()
    {
        return gScore + hScore;
    }

    public void AddNode(Node node)
    {
        connections.Add(node);
        Debug.Log("Node added to list.");
    }
}
