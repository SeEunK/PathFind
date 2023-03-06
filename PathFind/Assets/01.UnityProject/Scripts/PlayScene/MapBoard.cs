using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoard : MonoBehaviour
{
    private const string TERRAIN_MAP_OBJECT_NAME = "TerrainMap";

    public Vector2Int MapCellSize { get; private set; } = default;
    public Vector2 MapCellGap{ get;private set; } = default;

    private TerrainMap terrainMap = default;
    //
    private void Awake()
    {
        // �޴����� ��� �ʱ�ȭ �Ѵ�.
        ResManager.Instance.Create();

        //���� ������ �ʱ�ȭ�Ͽ� ��ġ�Ѵ�.
        terrainMap = gameObject.FindChildComponent<TerrainMap>(TERRAIN_MAP_OBJECT_NAME);
        terrainMap.InitAwake(this);

        MapCellSize = terrainMap.GetCellSize();
        MapCellGap = terrainMap.GetCellGap();
    }

}