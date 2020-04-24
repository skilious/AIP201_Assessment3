using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    public Transform seeker;
    public List<GameObject> checkpoints;
    grid SearchGrid;

    [HideInInspector]
    public List<Node> path;
    public List<Node> Unwalkable;

    void Awake()
    {
        SearchGrid = GetComponent<grid>();    
    }

    void Update()
    {
        SearchForPath(seeker.position, checkpoints[0].transform.position);
       
        float DistanceToNode0 = Vector3.Distance(seeker.position, checkpoints[0].transform.position);

        if(DistanceToNode0 <= 3.0f)
        {
            checkpoints.Add(checkpoints[0]);
            checkpoints.RemoveAt(0);
        }
    }

    void SearchForPath(Vector3 StartPos, Vector3 TargetPos)
    {
        Node StartNode = SearchGrid.NodeFromWorldPoint(StartPos);
        Node TargetNode = SearchGrid.NodeFromWorldPoint(TargetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(StartNode);

        while(openSet.Count > 0)
        {
            Node CurrentNode = openSet[0]; //current node is first in our list
            for(int i = 1; i < openSet.Count; i++) 
            {
                if(openSet[i].FCost < CurrentNode.FCost || openSet[i].FCost == CurrentNode.FCost && openSet[i].HCost < CurrentNode.HCost) 
                {
                    CurrentNode = openSet[i]; // work form the lowest cost node
                }
            }
            openSet.Remove(CurrentNode); //remove the node from the open list
            closedSet.Add(CurrentNode); //and add it to the closed list
            if (CurrentNode == TargetNode)
            {
                TreadPath(StartNode, TargetNode); //move along the path if it's our target
                return;
            }
            

            foreach(Node neighbour in SearchGrid.GetNeighbours(CurrentNode)) //work through all the surrounding nodes for our current node
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour)) //if it's not walkable, or we've already solved it, chuck it out
                {
                    continue;
                }

                int NewMovementCostToNeighbour = CurrentNode.GCost + GetDistance(CurrentNode, neighbour); //add GCOST to distance to neighbour
                if(NewMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) //if this is a faster path to the neighbour, or we haven't touched it before
                {
                    neighbour.GCost = NewMovementCostToNeighbour; //set the neighbour's GCOST
                    neighbour.HCost = GetDistance(neighbour, TargetNode); //calculate and set the  neighbour's HCOST
                    neighbour.parent = CurrentNode; //connect the dots
                    
                    if(!openSet.Contains(neighbour)) //if we've closed it
                    {
                        openSet.Add(neighbour); //re-add it
                    }
                }
            }
        }

    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.GridX - b.GridX); 
        int dstY = Mathf.Abs(a.GridY - b.GridY);

        if (dstX > dstY)
            return 14 * dstY * 10 * (dstX - dstY);
        else
            return 14 * dstX * 10 * (dstY - dstX);
        /*break our distance into X and Y components, 14  (1.4 = sqrt 2), or 10 for straight line -- this favours straight lines, and minimises the diagonals taken */
    }


    void TreadPath(Node startNode, Node EndNode)
    {
        path = new List<Node>(); //create path
        Node currentNode = EndNode; //start at the end

        while(currentNode != startNode) //work backwards
        {
            path.Add(currentNode); //setting each node in place
            currentNode = currentNode.parent; //and moving along
        }
        path.Reverse(); //then we reverse the list so it's right side forward

        SearchGrid.path = path; //and hand it to the grid code
    }

}
