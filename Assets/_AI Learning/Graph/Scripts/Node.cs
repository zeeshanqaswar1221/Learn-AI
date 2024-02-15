using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IComparable
{
    public List<Edge> edgeList = new List<Edge>();
    public Node path = null;
    public GameObject id;

    public Node parentNode;
    public float f, g, h;

    public Node(GameObject i)
    {
        id = i;
        path = null;
    }

    public int CompareTo(object obj)
    {
        if (obj.GetType() == typeof(Node)) // if incoming object is node
        {
            if (f > ((Node)obj).f)
            {
                return 1;
            }
            else if (f < ((Node)obj).f)
            {
                return -1;
            }
            
            return 0;

        }

        return 0;
    }

}
