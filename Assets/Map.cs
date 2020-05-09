using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    const float DIAGONAL_COST = 14;
    const float STRAIGHT_COST = 10;
    public List<WorldBlock> OpenList;
    public List<WorldBlock> ClosedList;
    public List<WorldBlock> AllBlocks;
    Stack<WorldBlock> path;
    public bool pathFindSucceed = false;
    public bool visualPathFindingDone = false;
    [SerializeField] public bool visualEffect = true;
    [SerializeField] Material closedMat = null;
    [SerializeField] Material openMat = null;
    [SerializeField] Material noneMat = null;
    [Range(1f, 100f)][SerializeField] float speed = 20f;


    private void Start() {
        WorldBlock[] tmp = FindObjectsOfType<WorldBlock>();
        foreach(WorldBlock wb in tmp)
        {
            AllBlocks.Add(wb);
            wb.GetComponent<MeshRenderer>().material = noneMat;
        }
        
    }

    public WorldBlock GetBlock(Vector3 pos)
    {
        foreach(WorldBlock wb in AllBlocks)
        {
            if(Vector3.Distance(wb.transform.position, pos) <= 0.4f)
            {
                return wb;
            }
        }

        return null;
    }
    public List<WorldBlock> PathFinding(WorldBlock start, WorldBlock destination)
    {
        foreach(WorldBlock wb in AllBlocks)
        {
            wb.GetComponent<MeshRenderer>().material = noneMat;
        }


        if(visualEffect)
        {
            return null;
        }
        else{
            OpenList = new List<WorldBlock>();
            ClosedList = new List<WorldBlock>();

            OpenList.Add(start);

            foreach(WorldBlock wb in AllBlocks)
            {
                wb.GCost = Mathf.Infinity;
                wb.previousNode = null;
            }

            // setting start node
            start.GCost = 0f;
            start.HCost = CalculateDistanceCost(start, destination);
            start.CalculateFValue();


            //diggin down to end node
            while(OpenList.Count > 0)
            {
                WorldBlock currentNode = GetLowestFCostNode(OpenList);

                //reached the destination
                if(currentNode == destination)
                    return CalculatePath(destination);
                

                OpenList.Remove(currentNode);
                ClosedList.Add(currentNode);


                foreach(WorldBlock neighborNode in currentNode.neighborNodes)
                {
                    if(ClosedList.Contains(neighborNode)) continue;

                    float tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighborNode);

                    if(tentativeGCost < neighborNode.GCost)
                    {
                        neighborNode.previousNode = currentNode;
                        neighborNode.GCost = tentativeGCost;
                        neighborNode.HCost = CalculateDistanceCost(neighborNode, destination);
                        neighborNode.CalculateFValue();

                        if(!OpenList.Contains(neighborNode))
                            OpenList.Add(neighborNode);                
                    }


                }
            }

            //Can't Find destination
            return null;
        }
    }

    private List<WorldBlock> CalculatePath(WorldBlock destination)
    {
        List<WorldBlock> path = new List<WorldBlock>();

        path.Add(destination);
        WorldBlock currentNode = destination;
        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }


    private float CalculateDistanceCost(WorldBlock a, WorldBlock b)
    {
        float xDist = Mathf.Abs(a.transform.position.x - b.transform.position.x);
        float yDist = Mathf.Abs(a.transform.position.y - b.transform.position.y);
        float remaining = Mathf.Abs(xDist - yDist);
        return DIAGONAL_COST * Mathf.Min(xDist, yDist) + STRAIGHT_COST * remaining;
    }

    private WorldBlock GetLowestFCostNode(List<WorldBlock> BlockList)
    {
        WorldBlock lowestFCostBlock = BlockList[0];

        foreach(WorldBlock wb in BlockList)
        {
            if(wb.FCost < lowestFCostBlock.FCost)
                lowestFCostBlock = wb;
        }

        return lowestFCostBlock;
    }


}
