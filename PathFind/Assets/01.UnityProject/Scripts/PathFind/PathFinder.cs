using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder :GSingleton<PathFinder>
{
    #region 지형 탐색을 위한 변수
    public GameObject sourceObj = default;
    public GameObject destinationObj = default;
    public MapBoard mapBoard = default;
    #endregion

    #region A star 알고리즘으로 최단 거리를 찾기위한 변수
    private List<AstarNode> aStarResultPath = default;
    private List<AstarNode> aStarOpenPath = default;
    private List<AstarNode> aStarClosePath = default;
    #endregion


    //출발지와 목적지 정보로 길을 찾는 함수

    public void FindPathAstar()
    {
        StartCoroutine(DelayFindPathAstar(1.0f));
    }

    // 탐색 알고리즘에 딜레이를 건다.
    private IEnumerator DelayFindPathAstar(float delay)
    {
        // Astar 알고리즘을 사용하기 위해서 path list 를 초기화한다
        aStarOpenPath = new List<AstarNode>();
        aStarClosePath = new List<AstarNode>();
        aStarResultPath = new List<AstarNode>();

        TerrainController targetTerrain = default;

        //출발지의 인덱스를 구해서, 출발지 노드를 찾아온다.
        string[] sourceObjNameParts = sourceObj.name.Split('_');
        int sourceIndex1D = -1;
        int.TryParse(sourceObjNameParts[sourceObjNameParts.Length - 1], out sourceIndex1D);
        targetTerrain = mapBoard.GetTerrain(sourceIndex1D);
        //찾아온 출발지 노드를 open 리스트에 추가
        AstarNode targetNode = new AstarNode(targetTerrain, destinationObj);
        AddAstarOpenList(targetNode);

        int loopIndex = 0;
        bool isFoundDestination = false;
        bool isNowayToGo = false;

        // A star 알고리즘으로 길을 찾는 메인 루프
        while (isFoundDestination == false && isNowayToGo == false)
        //while (loopIndex < 10)
        {
            //open list 를 순회해서 가장 cost 가 낮은 노드를 선택하는 로직
            AstarNode minCostNode = default;
            // 가장 cost 가 낮은 노드를 찾는 루프
            foreach (var terrainNode in aStarOpenPath)
            {
                if (minCostNode == default)
                {
                    minCostNode = terrainNode;
                }
                else
                {
                    // 가장 작은 코스트의 노드가 캐싱되어잇는 경우
                    // terrainNode 가 더 작은 코스트를 가지는 경우, mincostNode 를 업데이트 한다.
                    if (terrainNode.AstarF < minCostNode.AstarF)
                    {
                        minCostNode = terrainNode;
                    }
                    else { continue; }
                }
            }

            minCostNode.ShowCostAstar();
            minCostNode.terrain.SetTileActiveColor(RDefine.TileStatusColor.Search);

            //선택한 노드가 목적지에 도달했는지 확인하는 로직
            bool isArriveDest = mapBoard.GetDistance2D(minCostNode.terrain.gameObject, destinationObj).Equals(Vector2Int.zero);

            if (isArriveDest)
            {
                //목적지에 도착했다면, astarResultList 를 설정한다.
                AstarNode resultNode = minCostNode;
                bool isSetAstarResultPathOk = false;

                //이전 노드를 찾지 못할때 까지 순회하는 루프
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

                //open list 와 close list 를 정리한다.
                aStarOpenPath.Clear();
                aStarClosePath.Clear();
                isFoundDestination = true;
                break;
            }
            else
            {
                //도착 하지 않았다면, 현재 타일을 기준으로 4방향 노드를 찾아온다.
                List<int> nextSearchIndex1Ds = mapBoard.GetTileIdx2DAround4Ways(minCostNode.terrain.TileIdx2D);

                //찾아온 노드 중 이동 가능한 노드는 open list 에 추가한다.
                AstarNode nextNode = default;
                foreach (var nextIndex1D in nextSearchIndex1Ds)
                {
                    nextNode = new AstarNode(mapBoard.GetTerrain(nextIndex1D), destinationObj);
                    if (nextNode.terrain.IsPassable == false) { continue; }

                    AddAstarOpenList(nextNode, minCostNode);
                }

                // 탐색이 끝난 노드는 close list 에 추가하고, open list 에서 제거한다.
                // 이때, open list 가 비어있다면 더이상 탐색할수있는 길이 존재하지않는 것이다.
                aStarClosePath.Add(minCostNode);
                aStarOpenPath.Remove(minCostNode);

                //목적지에 도착하지 못했는데, 더이상 탐색할수 있는 길이 없는 경우
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



    // 비용을 설정한 노드를 open list 에 추가한다.
    private void AddAstarOpenList(AstarNode targetTerrain_, AstarNode prevNode = default)
    {
        //open 리스트에 추가하기 전에 알고리즘 비용을 설정한다.
        UpdateAstarCostToTerrain( targetTerrain_, prevNode);

        AstarNode closeNode = aStarClosePath.FindNode(targetTerrain_);
        if(closeNode !=default && closeNode != null)
        {
            /*do nothing*/
            //close list 에 이미 탐색이 끝난 좌표의 노가 존재하는 경우에는 openlist 에 추가하지않는다.
        }
        else 
        {
            // 아직 탐색이 끝나지 않은 노드인 경우.
            AstarNode openNode = aStarOpenPath.FindNode(targetTerrain_);
            //open list 에 현재 추가할 노드와 같은 좌표의 노드가 존재하는 경우
            if(openNode != default && openNode != null)
            {
                //타겟 노드의 코스트가 더 작은 경우에는 open list 에서 노드를 교체한다.
                if (targetTerrain_.AstarF < openNode.AstarF)
                {
                    aStarOpenPath.Remove(openNode);
                    aStarOpenPath.Add(targetTerrain_);
                }
                //타겟 노드의 코스트가 더 큰 경우에는 open list 에서 노드에 추가하지않는다.
                else  { /*do nothing*/ }
            }
            else
            {
                // open list 현재 추가할 노드와 같은 좌표의 노드가 없는 경우
                aStarOpenPath.Add(targetTerrain_);
            }
        }
    }
    
    
    // target 지형 정보와 destination 지형 정보로 distance와 heutistic을 설정하는 함수
    private void UpdateAstarCostToTerrain(AstarNode targetNode, AstarNode prevNode)
    {
        // target 지형에서 destination 까지의 2D 타일 거리를 계산하는 로직.
        Vector2Int distance2D = mapBoard.GetDistance2D(targetNode.terrain.gameObject, destinationObj);
        int totalDistance2D = distance2D.x + distance2D.y;

        // heuristic 은 직선 거리로 고정한다.
        Vector2 localDistance = destinationObj.transform.localPosition - targetNode.terrain.transform.localPosition;
        float heuristic = Mathf.Abs(localDistance.magnitude);

        // 이전 노드가 존재하는 경우 이전 노드의 코스트를 추가해서 연산한다.

        if(prevNode == default || prevNode == null)
        { /* nothing */ }
        else 
        {
            totalDistance2D = Mathf.RoundToInt(prevNode.AstarG + 1.0f);
        }
        
        targetNode.UpdateCostAstar(totalDistance2D, heuristic, prevNode);

    }


}
