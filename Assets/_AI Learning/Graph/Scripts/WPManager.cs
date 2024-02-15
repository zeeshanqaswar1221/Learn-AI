using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Link
{
    public enum direction {UNI, BI}

    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

public class WPManager : MonoBehaviour
{
    public Material startMat, expMat, pathNode, endMat;
    public List<Transform> wayPoints;
    public List<Link> links;
    
    private Graph m_Graph;

    private void Start()
    {
        m_Graph = new Graph();
        if (wayPoints.Count > 0)
        {
            foreach (var point in wayPoints)
            {
                m_Graph.AddNode(point.gameObject);
            }

            foreach (var l in links)
            {
                m_Graph.AddEdge(l.node1, l.node2);
                if (l.dir == Link.direction.BI)
                {
                    m_Graph.AddEdge(l.node2, l.node1);
                }
            }
        }
        
        Node start = m_Graph.nodes[UnityEngine.Random.Range(0, m_Graph.nodes.Count - 1)];
        Node end = m_Graph.nodes[UnityEngine.Random.Range(0, m_Graph.nodes.Count - 1)];

        Debug.Log($"Generate Path from {start.id.name} to {end.id.name}");
        if (m_Graph.Astar(start, end))
        {
            m_Graph.DrawPath(pathNode);
        }
    }

    private void OnDrawGizmos()
    {
        if (m_Graph != null && m_Graph.pathList.Count > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < m_Graph.pathList.Count - 1; i++)
            {
                Gizmos.DrawLine(m_Graph.pathList[i].id.transform.position, m_Graph.pathList[i+1].id.transform.position);
            }   
        }
    }
}
