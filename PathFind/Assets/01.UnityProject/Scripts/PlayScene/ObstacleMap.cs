using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : TileMapController
{
    private const string OBSTACLE_TILEMAP_OBJ_NAME = "ObstacleTilemap";

    private GameObject[] castleObjs = default; //길찾기 알고리즘을 테스트 할 출발지와 목적지를 캐싱할 오브젝트 배열


    //awake 타임에 초기화 할 내용을 재정의한다.

    public override void InitAwake(MapBoard mapController_)
    {
        this.tileMapObjName = OBSTACLE_TILEMAP_OBJ_NAME; //!!!!
        base.InitAwake(mapController_); //!!!!!
    }

    private void Start()
    {
        StartCoroutine(DelayStart(0f));
    }

    private IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        DoStart();
    }
    private void DoStart()
    {
        //출발지와 목적지를 설정해서 타일을 배치한다.
        castleObjs = new GameObject[2];
        TerrainController[] passableTerrains = new TerrainController[2];

        List<TerrainController> searchTerrains = default;

        int searchIndex = 0;
        TerrainController foundTile = default;

        //출발지는 좌측 -> 우측으로 y축을 서치해서 빈 지형을 받아온다.
        searchIndex= 0;
        foundTile = default;
        while (foundTile == null || foundTile == default)
        {
            searchTerrains = mapController.GetTerrainsColumn(searchIndex, true);

            foreach (var searchTerrain in searchTerrains)
            {
                if (searchTerrain.IsPassable)
                {
                    foundTile = searchTerrain;
                    break;
                }
            }

            if (foundTile != null || foundTile != default)
            {
                break;
            }
            if (mapController.MapCellSize.x - 1 <= searchIndex)
            {
                break;
            }
            searchIndex++;
        }
        passableTerrains[0] = foundTile;


        //목적지는 우측 -> 좌측 으로 y축을 서치해서 빈 지형을 받아온다.
        searchIndex = mapController.MapCellSize.x - 1;
        foundTile= default;

        while(foundTile == null || foundTile == default)
        {
            searchTerrains = mapController.GetTerrainsColumn(searchIndex);
            foreach(var searchTerrain in searchTerrains)
            {
                if(searchTerrain.IsPassable)
                {
                    foundTile = searchTerrain;
                    break;
                }

            }
            if(foundTile !=null|| foundTile != default)
            {
                break;
            }
            if ( searchIndex <= 0)
            {
                break;
            }
            searchIndex--;
        }
        passableTerrains[1] = foundTile;


        //출발지와 목적지에 지물을 추가한다.
        GameObject changeTilePrefab = ResManager.Instance.obstaclePrefabs[RDefine.OBSTACLE_PREF_PLAIN_CASTLE];
        GameObject tempChangeTile = default;

        //출발지 와 목적지를 인스턴스화 해서 캐싱하는 루프
        for (int i = 0; i < 2; i++)
        {
            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempChangeTile.name = string.Format("{0}_{1}", changeTilePrefab.name, passableTerrains[i].TileIdx1D);


            tempChangeTile.SetLocalScale(passableTerrains[i].transform.localScale);
            tempChangeTile.SetLocalPos(passableTerrains[i].transform.localPosition);

            //출발지와 목적지를 캐싱한다.
            castleObjs[i] = tempChangeTile;
            AddObstacle(tempChangeTile);
            
            tempChangeTile = default;
        }

        UpdateSourceDestinationToPathFinder();
    }

    // 지물을 추가한다.

    public void AddObstacle(GameObject obj)
    {
        allTileObjs.Add(obj);
    }

    // path finder 에 출발지와 목적지 설정한다.
    public void UpdateSourceDestinationToPathFinder()
    {
        PathFinder.Instance.sourceObj = castleObjs[0];
        PathFinder.Instance.destinationObj = castleObjs[1];
    }
}
