using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap : TileMapController
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "TerrainTileMap";

    private Vector2Int mapCellSize = default;
    private Vector2 mapCellGap = default;
    private List<TerrainController> allTerrains = default;


    // ! Awake 타임에 초기화 할 내용을 재정의한다.

    public override void InitAwake(MapBoard mapController_)
    {
        this.tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME; //!!!!!
        base.InitAwake(mapController_); //!!!!!

        allTerrains = new List<TerrainController>();

        //타일을 x축 갯수와 전체 타일의 수로 맵의 가로, 세로 사이즈를 연산한다.
        mapCellSize = Vector2Int.zero;
        float tempTileY = allTileObjs[0].transform.localPosition.y;

        for(int i = 0; i < allTileObjs.Count; i++)
        {
            if (tempTileY.IsEquals(allTileObjs[i].transform.localPosition.y) == false)
            {
                mapCellSize.x = i;
                break;
            }
        }

        //전체 타일의 수를 맵의 가로 셀 크기로 나눈 값이 맵의 세로 셀 크기이다.
        mapCellSize.y = Mathf.FloorToInt(allTileObjs.Count / mapCellSize.x);

        //x 축 상의 두 타일과 y축 상의 두 타일 사이의 로컬 포지션으로 타일 갭을 연산한다.
        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x - allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y - allTileObjs[0].transform.localPosition.y;

    }
    private void Start()
    {
        //타일 맵의 일부를 일정확률로 다른 타일로 교체하는 로직
        GameObject changeTilePrefab = ResManager.Instance.terrainPrefabs[RDefine.TERRAIN_PREF_OCEAN];

        //타일맵 중에 어느 정도를 바다로 교체할것인지 결정한다.

        const float CHANGE_PERCANTAGE = 15.0f;
       
        float currentChangePercentage = allTileObjs.Count * (CHANGE_PERCANTAGE / 100.0f);

        //바다로 교체할 타일의 정보를 리스트 형태로 생성해서 섞는다.
        List<int> changeTileResult = GFunc.CreateList(allTileObjs.Count, 1);
        changeTileResult.Shuffle();

        GameObject tempChangeTile = default;
        for(int i = 0; i< allTileObjs.Count; i++)
        {
            //위에서 연산한 정보로 현재 타일맵에 바다를 적용하는 루프
            if(currentChangePercentage <= changeTileResult[i])
            {
                continue;
            }
            // 프리팹을 인스턴스화 해서 교체할 타일의 트랜스폼을 카피한다.
            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempChangeTile.name = changeTilePrefab.name;
            tempChangeTile.SetLocalScale(allTileObjs[i].transform.localScale);
            tempChangeTile.SetLocalPos(allTileObjs[i].transform.localPosition);

            allTileObjs.Swap(ref tempChangeTile, i);
            tempChangeTile.DestroyObj();
        }

        //기존에 존재하는 타일의 순서를 조정하고, 컨트롤러를 캐싱하는 로직

        TerrainController tempTerrain = default;
        TerrainType type = TerrainType.NONE;

        int loopCnt = 0;
        foreach (GameObject tile_ in allTileObjs)
        {
            tempTerrain = tile_.GetComponentMust<TerrainController>();
            switch (tempTerrain.name)
            {
                case RDefine.TERRAIN_PREF_PLAIN:
                    type = TerrainType.PLAIN_PASS; 
                    break;
                case RDefine.TERRAIN_PREF_OCEAN:
                    type = TerrainType.OCEAN_N_PASS; 
                    break;
                default:
                    type = TerrainType.NONE; 
                    break;
            }
            tempTerrain.SetupTerrain(mapController,type,loopCnt);
            tempTerrain.transform.SetAsFirstSibling();
            allTerrains.Add(tempTerrain);
            loopCnt += 1;
        }//타일의 이름과 렌더링 순서래도 정렬하는 루프
    }

    //초기화된 타일의 정보로 연산한 맵의 가로, 세로 크기를 리턴한다.
    public Vector2Int GetCellSize()
    {
        return mapCellSize;
    }
    
    // 초기화된 타일의 정보로 연산된 타일 사이의 갭을 리턴한다.
    public Vector2 GetCellGap()
    {
        return mapCellGap;
    }

    // 인덱스에 해당하는 타일을 리턴한다.
    public TerrainController GetTile(int tileIndex)
    {
        if (allTerrains.IsValid(tileIndex))
        {
            return allTerrains[tileIndex];
        }
        return default;
    }
}
