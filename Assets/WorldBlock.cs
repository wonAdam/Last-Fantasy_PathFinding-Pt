using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBlock : MonoBehaviour
{
    public float GCost; // G
    public float HCost; // H
    public float FCost; // G + H
    public WorldBlock previousNode;
    public List<WorldBlock> neighborNodes;
    public Transform playerTr;
    [SerializeField] public bool canYouPass;

    private void Awake() {
        playerTr = FindObjectOfType<Player>().transform;
        SetNeighborNode();
    }

    public void CalculateFValue()
    {
        FCost = GCost + HCost;
    }

    private void SetNeighborNode()
    {
        WorldBlock[] AllWorldBlocks = FindObjectsOfType<WorldBlock>();

        foreach(WorldBlock wb in AllWorldBlocks)
        {   
            if(wb == this) continue;
            if(!wb.canYouPass) continue;

            //same x pos
            if(Mathf.Abs(wb.transform.position.x - transform.position.x) < Mathf.Epsilon)
            {
                if(Mathf.Abs(wb.transform.position.z - transform.position.z) - 1f < Mathf.Epsilon)
                {
                    neighborNodes.Add(wb);
                }
            }
            //same y pos
            else if(Mathf.Abs(wb.transform.position.z - transform.position.z) < Mathf.Epsilon)
            {
                if(Mathf.Abs(wb.transform.position.x - transform.position.x) - 1f < Mathf.Epsilon)
                {
                    neighborNodes.Add(wb);
                }
            }
            else if(Mathf.Abs(wb.transform.position.x - transform.position.x) - 1f < Mathf.Epsilon &&
                 Mathf.Abs(wb.transform.position.z - transform.position.z) - 1f < Mathf.Epsilon)  
            {
                    if(Mathf.Abs(wb.transform.position.x - transform.position.x) - 1f < Mathf.Epsilon)
                    {
                        neighborNodes.Add(wb);
                    }                 
            }      
        }
    }
}
