using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 GridWorldSize;
    public float nodeRadius;
    Node[,] WorldGrid;

    float nodeDiameter;
    int GridSizeX, GridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2; //this should be obvious
        GridSizeX = Mathf.RoundToInt( GridWorldSize.x / nodeDiameter); //find the biggest number we can fit
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter); //into our grids
        CreateGrid(); //then create the grid :D
    }


    public Node NodeFromWorldPoint(Vector3 WorldPos)
    {
        float PercentX = (WorldPos.x + GridWorldSize.x / 2) / GridWorldSize.x; //solve for our X location
        float PercentY = (WorldPos.z + GridWorldSize.y / 2) / GridWorldSize.y;  //and our Y location
        PercentX = Mathf.Clamp01(PercentX); //tehn make sure we can't have it outside the bounds
        PercentY = Mathf.Clamp01(PercentY); //clamp for safety 

        int x = Mathf.RoundToInt((GridSizeX - 1) * PercentX); //round to whole number and send it
        int y = Mathf.RoundToInt((GridSizeY - 1) * PercentY); //round to whole number and S E N D  I T

        while(!WorldGrid[x,y].walkable)
        {
            if(x < GridSizeX)
            {
                x++;
            }
            else if( x > GridSizeX)
            {
                x--;
            }
            if(y < GridSizeY)
            {
                y++;
            } 
            else if (y > GridSizeY)
            {
                y--;
            }
            
        }

        return WorldGrid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int CheckX = node.GridX + x;
                int CheckY = node.GridY + y;
                if(CheckX >= 0 && CheckX < GridSizeX && CheckY >= 0 && CheckY < GridSizeY)
                {
                    neighbours.Add(WorldGrid[CheckX, CheckY]);
                }
            }
        }
        return neighbours;
    }

   

    void CreateGrid()
    {
        WorldGrid = new Node[GridSizeX, GridSizeY];
        Vector3 WorldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y/2;
        for(int x = 0; x < GridSizeX; x++)
        {
            for(int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPoint = WorldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                WorldGrid[x, y] = new Node(walkable, worldPoint, x, y);

            }
        }
    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

        if (WorldGrid != null)
        {

            foreach (Node n in WorldGrid)
            {
                Color map = (n.walkable) ? Color.white : Color.red;
                map.a = 0.25f;
                Gizmos.color = map; 
                if(path != null)
                {
                    if (path.Contains(n))
                    {
                        Color path = Color.black;
                        path.a = 0.4f;
                        Gizmos.color = path;

                    }
                        
                }
                Gizmos.DrawCube(n.NodeWorldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

}
