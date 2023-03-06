using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : GSingleton<ResManager>
{
    private const string TERRAIN_PREFAB_PATH = "Prefabs/ObjectTiles/Terrains";
    private const string OBSTACLE_PREFAB_PATH = "Prefabs/ObjectTiles/Obstacle";


    public Dictionary<string, GameObject> terrainPrefabs = default;
    public Dictionary<string, GameObject> obstaclePrefabs = default;


    protected override void Init()
    {
        base.Init();

        terrainPrefabs = new Dictionary<string, GameObject>();
        obstaclePrefabs = new Dictionary<string, GameObject>();

        terrainPrefabs.AddObjs(Resources.LoadAll<GameObject>(TERRAIN_PREFAB_PATH));
        obstaclePrefabs.AddObjs(Resources.LoadAll<GameObject>(OBSTACLE_PREFAB_PATH));
    }

}
