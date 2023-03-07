using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoard : MonoBehaviour
{
    private const string TERRAIN_MAP_OBJECT_NAME = "TerrainMap";
    private const string OBSTACLE_MAP_OBJECT_NAME = "ObstacleMap";
    public Vector2Int MapCellSize { get; private set; } = default;
    public Vector2 MapCellGap{ get;private set; } = default;

    private TerrainMap terrainMap = default;
    private ObstacleMap obstacleMap = default;
    
    private void Awake()
    {
        // �޴����� ��� �ʱ�ȭ �Ѵ�.
        ResManager.Instance.Create();

        //���� ������ �ʱ�ȭ�Ͽ� ��ġ�Ѵ�.
        terrainMap = gameObject.FindChildComponent<TerrainMap>(TERRAIN_MAP_OBJECT_NAME);
        terrainMap.InitAwake(this);

        MapCellSize = terrainMap.GetCellSize();
        MapCellGap = terrainMap.GetCellGap();

        //�ʿ� ������ �ʱ�ȭ �Ͽ� ��ġ�Ѵ�.
        obstacleMap = gameObject.FindChildComponent<ObstacleMap>(OBSTACLE_MAP_OBJECT_NAME);
        obstacleMap.InitAwake(this);
    }

    //Ÿ�� �ε����� �޾Ƽ� �ش� Ÿ���� �����ϴ� �Լ�
    public TerrainController GetTerrain(int index1D)
    {
        return terrainMap.GetTile(index1D);
    }

    // ���� x ��ǥ�� �޾Ƽ� �ش� ���� Ÿ���� ����Ʈ�� �������� �Լ�
    public List<TerrainController> GetTerrainsColumn(int xIndex2D)
    {

        return GetTerrainsColumn(xIndex2D, false); 
    }

    public List<TerrainController> GetTerrainsColumn(int xIndex2D, bool isSortReverse)
    {
        List<TerrainController> terrains = new List<TerrainController>();
        TerrainController tempTile = default;

        int tileIdx1D = 0;

        for(int y = 0; y < MapCellSize.y; y++)
        {
            tileIdx1D = y * MapCellSize.x + xIndex2D;

            tempTile = terrainMap.GetTile(tileIdx1D);

            terrains.Add(tempTile);
        } //y���� ũ�⸸ŭ ��ȸ�ϴ� ����

        if (terrains.IsValid())
        {
            if (isSortReverse)
            {
                terrains.Reverse();
            }
            return terrains;
        }
        else
        {
            return default;
        }
    }

    //������ �ε����� 2d ��ǥ�� �����ϴ� �Լ�
    public Vector2Int GetTileIndex2D (int idx1D)
    {
        Vector2Int tileIdx2D = Vector2Int.zero;
        tileIdx2D.x = idx1D % MapCellSize.x;
        tileIdx2D.y = idx1D / MapCellSize.x;

        return tileIdx2D;
    }

    //������ 2d ��ǥ�� �ε����� �����ϴ� �Լ�
    public int GetTileIndex1D(Vector2Int idx2D)
    {
        int tileIdx1D = (idx2D.y * MapCellSize.x) + idx2D.x;
        return tileIdx1D;
    }

    // ������ ������ Ÿ�� �Ÿ��� �����ϴ� �Լ�

    public Vector2Int GetDistance2D(GameObject targetTerrainObj, GameObject destTerrainObj)
    {
        Vector2 localDistance = destTerrainObj.transform.localPosition - targetTerrainObj.transform.localPosition;

        Vector2Int distance2D = Vector2Int.zero;
        distance2D.x = Mathf.RoundToInt(localDistance.x / MapCellGap.x);
        distance2D.y = Mathf.RoundToInt(localDistance.y / MapCellGap.y);
        distance2D = GFunc.Abs(distance2D);

        return distance2D;
    }

    //2D ��ǥ�� �������� �ֺ� 4���� Ÿ���� �ε����� �����ϴ� �Լ�
    public List<int> GetTileIdx2DAround4Ways(Vector2Int targetIdx2D)
    {
        List<int> index1D_around4ways = new List<int>();
        List<Vector2Int> index2D_aroung4ways = new List<Vector2Int>();

        index2D_aroung4ways.Add(new Vector2Int(targetIdx2D.x - 1, targetIdx2D.y));
        index2D_aroung4ways.Add(new Vector2Int(targetIdx2D.x + 1, targetIdx2D.y));
        index2D_aroung4ways.Add(new Vector2Int(targetIdx2D.x , targetIdx2D.y -1 ));
        index2D_aroung4ways.Add(new Vector2Int(targetIdx2D.x , targetIdx2D.y +1 ));

        foreach(var index2D in index2D_aroung4ways) 
        {
            if(index2D.x.IsInRange(0, MapCellSize.x)== false)
            {
                continue;
            }
            if(index2D.y.IsInRange(0,MapCellSize.y)==false)
            {
                continue;
            }

            index1D_around4ways.Add(GetTileIndex1D(index2D));
        }

        return index1D_around4ways;
    }
}
