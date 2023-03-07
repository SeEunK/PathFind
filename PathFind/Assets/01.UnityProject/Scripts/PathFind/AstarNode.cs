using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode 
{
    public TerrainController terrain { get; private set; }
    public GameObject destinationObj { get; private set; }


    // A star algorithm
    public float AstarF { get; private set; } = float.MaxValue;
    public float AstarG { get; private set; } = float.MaxValue;
    public float AstarH { get; private set; } = float.MaxValue;
    public AstarNode AstarPrevNode { get; private set; } = default;
    public AstarNode(TerrainController terrain_, GameObject destinationObj_)
    {
        terrain = terrain_;
        destinationObj = destinationObj_;
    }


    public void UpdateCostAstar(float gCost, float heuristic, AstarNode prevNode)
    {
        float aStarF = gCost + heuristic;
        
        if(aStarF < AstarF)
        {
            AstarG = gCost;
            AstarH = heuristic;
            AstarF = aStarF;

            AstarPrevNode = prevNode;
        } // 비용이 더 작은 경우에만 업데이트 한다.
        else { /*nothing*/ }
    }

    public void ShowCostAstar()
    {
        Debug.LogFormat("TileIndex1D : {0}, F:{1}, G:{2}, H:{3}", terrain.TileIdx1D, AstarF, AstarG, AstarH);
    }

}
