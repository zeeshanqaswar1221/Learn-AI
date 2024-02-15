using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Graph
{
    public Node endNode;
    
    public List<Edge> edges = new List<Edge>();
    public List<Node> nodes = new List<Node>();
    public List<Node> pathList = new List<Node>();

    private List<Node> exploredNodes = new List<Node>();

    public void AddNode(GameObject g)
    {
        Node node = new Node(g);
        nodes.Add(node);
    }

    public void AddEdge(GameObject gf, GameObject gt)
    {
        Node fromNode = FindNode(gf);
        Node toNode = FindNode(gt);

        Edge edge = new Edge(fromNode, toNode);

        edges.Add(edge);
        fromNode.edgeList.Add(edge);
    }


    public bool Astar(Node startNode, Node goalNode)
    {
        if (startNode.id == goalNode.id)
        {
            Debug.Log("Standing point is your path!");
            return false;
        }

        startNode.g = startNode.h = 0f;
        pathList.Add(startNode);

        // Not consume if it already exist in Explored Nodes & pathList
        while (pathList.Count > 0)
        {
            Node expNode = GetLowestF(pathList);
        
            exploredNodes.Add(expNode);
            pathList.Remove(expNode);
            
            foreach (var edge in expNode.edgeList)
            {
                if (!exploredNodes.Contains(edge.endNode) && !pathList.Contains(edge.endNode))
                {
                    edge.endNode.g = Distance(edge.endNode, goalNode);
                    edge.endNode.h = Distance(edge.endNode, expNode);
                    edge.endNode.f = edge.endNode.g + edge.endNode.h;
        
                    edge.endNode.parentNode = expNode;
                    pathList.Add(edge.endNode);
        
                    if (edge.endNode == goalNode)
                    {
                        Debug.Log($"Path Found" );
                        endNode = edge.endNode;
                        startNode.parentNode = null;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void DrawPath(Material mat)
    {
        pathList = new List<Node>();
        
        Node pathNode = endNode;
        while (pathNode != null)
        {
            pathList.Add(pathNode);
            pathNode.id.GetComponent<Renderer>().sharedMaterial = mat; 
            Debug.Log(pathNode.id.name);
            pathNode = pathNode.parentNode;
        }
    }

    public Node FindNode(GameObject id)
    {
        foreach (var n in nodes)
        {
            if (n.id == id)
            {
                return n;
            }
        }

        return null;
    }

    public float Distance(Node a, Node b)
    {
        return Vector3.SqrMagnitude(b.id.transform.position - a.id.transform.position);
    }

    public Node GetLowestF(List<Node> nodeList)
    {
        nodeList.Sort();
        return nodeList[0];
    }

}
