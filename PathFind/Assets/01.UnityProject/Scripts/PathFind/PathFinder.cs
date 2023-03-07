using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder :GSingleton<PathFinder>
{
    #region ���� Ž���� ���� ����
    public GameObject sourceObj = default;
    public GameObject destinationObj = default;
    public MapBoard mapBoard = default;
    #endregion

    #region A star �˰������� �ִ� �Ÿ��� ã������ ����
    private List<AstarNode> aStarResultPath = default;
    private List<AstarNode> aStarOpenPath = default;
    private List<AstarNode> aStarClosePath = default;
    #endregion


    //������� ������ ������ ���� ã�� �Լ�

    public void FindPathAstar()
    {
        StartCoroutine(DelayFindPathAstar(1.0f));
    }

    // Ž�� �˰��� �����̸� �Ǵ�.
    private IEnumerator DelayFindPathAstar(float delay)
    {
        // Astar �˰����� ����ϱ� ���ؼ� path list �� �ʱ�ȭ�Ѵ�
        aStarOpenPath = new List<AstarNode>();
        aStarClosePath = new List<AstarNode>();
        aStarResultPath = new List<AstarNode>();

        TerrainController targetTerrain = default;

        //������� �ε����� ���ؼ�, ����� ��带 ã�ƿ´�.
        string[] sourceObjNameParts = sourceObj.name.Split('_');
        int sourceIndex1D = -1;
        int.TryParse(sourceObjNameParts[sourceObjNameParts.Length - 1], out sourceIndex1D);
        targetTerrain = mapBoard.GetTerrain(sourceIndex1D);
        //ã�ƿ� ����� ��带 open ����Ʈ�� �߰�
        AstarNode targetNode = new AstarNode(targetTerrain, destinationObj);
        AddAstarOpenList(targetNode);

        int loopIndex = 0;
        bool isFoundDestination = false;
        bool isNowayToGo = false;

        // A star �˰������� ���� ã�� ���� ����
        while (isFoundDestination == false && isNowayToGo == false)
        //while (loopIndex < 10)
        {
            //open list �� ��ȸ�ؼ� ���� cost �� ���� ��带 �����ϴ� ����
            AstarNode minCostNode = default;
            // ���� cost �� ���� ��带 ã�� ����
            foreach (var terrainNode in aStarOpenPath)
            {
                if (minCostNode == default)
                {
                    minCostNode = terrainNode;
                }
                else
                {
                    // ���� ���� �ڽ�Ʈ�� ��尡 ĳ�̵Ǿ��մ� ���
                    // terrainNode �� �� ���� �ڽ�Ʈ�� ������ ���, mincostNode �� ������Ʈ �Ѵ�.
                    if (terrainNode.AstarF < minCostNode.AstarF)
                    {
                        minCostNode = terrainNode;
                    }
                    else { continue; }
                }
            }

            minCostNode.ShowCostAstar();
            minCostNode.terrain.SetTileActiveColor(RDefine.TileStatusColor.Search);

            //������ ��尡 �������� �����ߴ��� Ȯ���ϴ� ����
            bool isArriveDest = mapBoard.GetDistance2D(minCostNode.terrain.gameObject, destinationObj).Equals(Vector2Int.zero);

            if (isArriveDest)
            {
                //�������� �����ߴٸ�, astarResultList �� �����Ѵ�.
                AstarNode resultNode = minCostNode;
                bool isSetAstarResultPathOk = false;

                //���� ��带 ã�� ���Ҷ� ���� ��ȸ�ϴ� ����
                while (isSetAstarResultPathOk == false)
                {
                    aStarResultPath.Add(resultNode);
                    if (resultNode.AstarPrevNode == default || resultNode.AstarPrevNode == null)
                    {
                        isSetAstarResultPathOk = true;
                        break;
                    }
                    else { /*do nothing*/}

                    resultNode = resultNode.AstarPrevNode;
                }

                //open list �� close list �� �����Ѵ�.
                aStarOpenPath.Clear();
                aStarClosePath.Clear();
                isFoundDestination = true;
                break;
            }
            else
            {
                //���� ���� �ʾҴٸ�, ���� Ÿ���� �������� 4���� ��带 ã�ƿ´�.
                List<int> nextSearchIndex1Ds = mapBoard.GetTileIdx2DAround4Ways(minCostNode.terrain.TileIdx2D);

                //ã�ƿ� ��� �� �̵� ������ ���� open list �� �߰��Ѵ�.
                AstarNode nextNode = default;
                foreach (var nextIndex1D in nextSearchIndex1Ds)
                {
                    nextNode = new AstarNode(mapBoard.GetTerrain(nextIndex1D), destinationObj);
                    if (nextNode.terrain.IsPassable == false) { continue; }

                    AddAstarOpenList(nextNode, minCostNode);
                }

                // Ž���� ���� ���� close list �� �߰��ϰ�, open list ���� �����Ѵ�.
                // �̶�, open list �� ����ִٸ� ���̻� Ž���Ҽ��ִ� ���� ���������ʴ� ���̴�.
                aStarClosePath.Add(minCostNode);
                aStarOpenPath.Remove(minCostNode);

                //�������� �������� ���ߴµ�, ���̻� Ž���Ҽ� �ִ� ���� ���� ���
                if (aStarOpenPath.IsValid() == false)
                {
                    Debug.LogFormat("[Warning] There are no more tiles to explore.");
                    isNowayToGo = true;
                }


                foreach (var tempNode in aStarOpenPath)
                {
                    Debug.LogFormat("index : {0}, Cost :{1}", tempNode.terrain.TileIdx1D, tempNode.AstarF);
                }
            }

            loopIndex++;
            yield return new WaitForSeconds(delay);

        }
    }



    // ����� ������ ��带 open list �� �߰��Ѵ�.
    private void AddAstarOpenList(AstarNode targetTerrain_, AstarNode prevNode = default)
    {
        //open ����Ʈ�� �߰��ϱ� ���� �˰��� ����� �����Ѵ�.
        UpdateAstarCostToTerrain( targetTerrain_, prevNode);

        AstarNode closeNode = aStarClosePath.FindNode(targetTerrain_);
        if(closeNode !=default && closeNode != null)
        {
            /*do nothing*/
            //close list �� �̹� Ž���� ���� ��ǥ�� �밡 �����ϴ� ��쿡�� openlist �� �߰������ʴ´�.
        }
        else 
        {
            // ���� Ž���� ������ ���� ����� ���.
            AstarNode openNode = aStarOpenPath.FindNode(targetTerrain_);
            //open list �� ���� �߰��� ���� ���� ��ǥ�� ��尡 �����ϴ� ���
            if(openNode != default && openNode != null)
            {
                //Ÿ�� ����� �ڽ�Ʈ�� �� ���� ��쿡�� open list ���� ��带 ��ü�Ѵ�.
                if (targetTerrain_.AstarF < openNode.AstarF)
                {
                    aStarOpenPath.Remove(openNode);
                    aStarOpenPath.Add(targetTerrain_);
                }
                //Ÿ�� ����� �ڽ�Ʈ�� �� ū ��쿡�� open list ���� ��忡 �߰������ʴ´�.
                else  { /*do nothing*/ }
            }
            else
            {
                // open list ���� �߰��� ���� ���� ��ǥ�� ��尡 ���� ���
                aStarOpenPath.Add(targetTerrain_);
            }
        }
    }
    
    
    // target ���� ������ destination ���� ������ distance�� heutistic�� �����ϴ� �Լ�
    private void UpdateAstarCostToTerrain(AstarNode targetNode, AstarNode prevNode)
    {
        // target �������� destination ������ 2D Ÿ�� �Ÿ��� ����ϴ� ����.
        Vector2Int distance2D = mapBoard.GetDistance2D(targetNode.terrain.gameObject, destinationObj);
        int totalDistance2D = distance2D.x + distance2D.y;

        // heuristic �� ���� �Ÿ��� �����Ѵ�.
        Vector2 localDistance = destinationObj.transform.localPosition - targetNode.terrain.transform.localPosition;
        float heuristic = Mathf.Abs(localDistance.magnitude);

        // ���� ��尡 �����ϴ� ��� ���� ����� �ڽ�Ʈ�� �߰��ؼ� �����Ѵ�.

        if(prevNode == default || prevNode == null)
        { /* nothing */ }
        else 
        {
            totalDistance2D = Mathf.RoundToInt(prevNode.AstarG + 1.0f);
        }
        
        targetNode.UpdateCostAstar(totalDistance2D, heuristic, prevNode);

    }


}
