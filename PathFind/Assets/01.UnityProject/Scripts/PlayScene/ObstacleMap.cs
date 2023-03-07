using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : TileMapController
{
    private const string OBSTACLE_TILEMAP_OBJ_NAME = "ObstacleTilemap";

    private GameObject[] castleObjs = default; //��ã�� �˰����� �׽�Ʈ �� ������� �������� ĳ���� ������Ʈ �迭


    //awake Ÿ�ӿ� �ʱ�ȭ �� ������ �������Ѵ�.

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
        //������� �������� �����ؼ� Ÿ���� ��ġ�Ѵ�.
        castleObjs = new GameObject[2];
        TerrainController[] passableTerrains = new TerrainController[2];

        List<TerrainController> searchTerrains = default;

        int searchIndex = 0;
        TerrainController foundTile = default;

        //������� ���� -> �������� y���� ��ġ�ؼ� �� ������ �޾ƿ´�.
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


        //�������� ���� -> ���� ���� y���� ��ġ�ؼ� �� ������ �޾ƿ´�.
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


        //������� �������� ������ �߰��Ѵ�.
        GameObject changeTilePrefab = ResManager.Instance.obstaclePrefabs[RDefine.OBSTACLE_PREF_PLAIN_CASTLE];
        GameObject tempChangeTile = default;

        //����� �� �������� �ν��Ͻ�ȭ �ؼ� ĳ���ϴ� ����
        for (int i = 0; i < 2; i++)
        {
            tempChangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempChangeTile.name = string.Format("{0}_{1}", changeTilePrefab.name, passableTerrains[i].TileIdx1D);


            tempChangeTile.SetLocalScale(passableTerrains[i].transform.localScale);
            tempChangeTile.SetLocalPos(passableTerrains[i].transform.localPosition);

            //������� �������� ĳ���Ѵ�.
            castleObjs[i] = tempChangeTile;
            AddObstacle(tempChangeTile);
            
            tempChangeTile = default;
        }

        UpdateSourceDestinationToPathFinder();
    }

    // ������ �߰��Ѵ�.

    public void AddObstacle(GameObject obj)
    {
        allTileObjs.Add(obj);
    }

    // path finder �� ������� ������ �����Ѵ�.
    public void UpdateSourceDestinationToPathFinder()
    {
        PathFinder.Instance.sourceObj = castleObjs[0];
        PathFinder.Instance.destinationObj = castleObjs[1];
    }
}
