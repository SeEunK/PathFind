using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap : TileMapController
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "TerrainTileMap";

    private Vector2Int mapCellSize = default;
    private Vector2 mapCellGap = default;
    private List<TerrainController> allTerrains = default;


    // ! Awake Ÿ�ӿ� �ʱ�ȭ �� ������ �������Ѵ�.

    public override void InitAwake(MapBoard mapController_)
    {
        this.tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME; //!!!!!
        base.InitAwake(mapController_); //!!!!!

        allTerrains = new List<TerrainController>();

        //Ÿ���� x�� ������ ��ü Ÿ���� ���� ���� ����, ���� ����� �����Ѵ�.
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

        //��ü Ÿ���� ���� ���� ���� �� ũ��� ���� ���� ���� ���� �� ũ���̴�.
        mapCellSize.y = Mathf.FloorToInt(allTileObjs.Count / mapCellSize.x);

        //x �� ���� �� Ÿ�ϰ� y�� ���� �� Ÿ�� ������ ���� ���������� Ÿ�� ���� �����Ѵ�.
        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x - allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y - allTileObjs[0].transform.localPosition.y;

    }
    private void Start()
    {
        //Ÿ�� ���� �Ϻθ� ����Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����
        GameObject changeTilePrefab = ResManager.Instance.terrainPrefabs[RDefine.TERRAIN_PREF_OCEAN];

        //Ÿ�ϸ� �߿� ��� ������ �ٴٷ� ��ü�Ұ����� �����Ѵ�.

        const float CHANGE_PERCANTAGE = 15.0f;
       
        float currentChangePercentage = allTileObjs.Count * (CHANGE_PERCANTAGE / 100.0f);

        //�ٴٷ� ��ü�� Ÿ���� ������ ����Ʈ ���·� �����ؼ� ���´�.
        List<int> changeTileResult = GFunc.CreateList(allTileObjs.Count, 1);
        changeTileResult.Shuffle();

        GameObject tempChangeTile = default;
        for(int i = 0; i< allTileObjs.Count; i++)
        {
            //������ ������ ������ ���� Ÿ�ϸʿ� �ٴٸ� �����ϴ� ����
            if(currentChangePercentage <= changeTileResult[i])
            {
                continue;
            }
            // �������� �ν��Ͻ�ȭ �ؼ� ��ü�� Ÿ���� Ʈ�������� ī���Ѵ�.
            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempChangeTile.name = changeTilePrefab.name;
            tempChangeTile.SetLocalScale(allTileObjs[i].transform.localScale);
            tempChangeTile.SetLocalPos(allTileObjs[i].transform.localPosition);

            allTileObjs.Swap(ref tempChangeTile, i);
            tempChangeTile.DestroyObj();
        }

        //������ �����ϴ� Ÿ���� ������ �����ϰ�, ��Ʈ�ѷ��� ĳ���ϴ� ����

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
        }//Ÿ���� �̸��� ������ �������� �����ϴ� ����
    }

    //�ʱ�ȭ�� Ÿ���� ������ ������ ���� ����, ���� ũ�⸦ �����Ѵ�.
    public Vector2Int GetCellSize()
    {
        return mapCellSize;
    }
    
    // �ʱ�ȭ�� Ÿ���� ������ ����� Ÿ�� ������ ���� �����Ѵ�.
    public Vector2 GetCellGap()
    {
        return mapCellGap;
    }

    // �ε����� �ش��ϴ� Ÿ���� �����Ѵ�.
    public TerrainController GetTile(int tileIndex)
    {
        if (allTerrains.IsValid(tileIndex))
        {
            return allTerrains[tileIndex];
        }
        return default;
    }
}
