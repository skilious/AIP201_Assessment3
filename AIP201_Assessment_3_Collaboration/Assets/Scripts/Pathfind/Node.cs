using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{  //simple storage script for data for each 'node' in the world -- only two functions; one constructor and one for calculating H cost
    public bool walkable;
    public Vector3 NodeWorldPos;
    public int GCost;
    public int HCost;
    public int GridX;
    public int GridY;

    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridx, int _gridy)
    {
        walkable = _walkable;
        NodeWorldPos = _worldPos;
        GridX = _gridx;
        GridY = _gridy;
    }

    public int FCost
    {
        get { 
            return GCost + HCost;
        }
    }

}
