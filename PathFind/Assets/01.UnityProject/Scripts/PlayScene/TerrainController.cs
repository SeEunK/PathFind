using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    private const string TILE_FRONT_RENDERER_OBJ_NAME = "FrontRenderer";

    private TerrainType terrainType = TerrainType.NONE;
    private MapBoard mapController = default;

    public bool IsPassable { get; private set; } = false;
    public int TileIdx1D { get; private set; } = -1;
    public Vector2Int TileIdx2D { get; private set; } = default;

    #region 길찾기 알고리즘을 위한 변수
    private SpriteRenderer frontRenderer = default;
    private Color defaultColor = default;
    private Color selectedColor = default;
    private Color searchColor = default;
    private Color inacticveColor = default;
    #endregion // 길찾기 알고리즘을 위한 변수


    private void Awake()
    {
        frontRenderer = gameObject.FindChildComponent<SpriteRenderer>(TILE_FRONT_RENDERER_OBJ_NAME);

        GFunc.Assert(frontRenderer != null || frontRenderer != default);

        defaultColor = new Color(1.0f,1.0f, 1.0f, 1.0f);
        selectedColor = new Color(236f / 255f, 130f / 255f, 20f / 255f, 1.0f);
        searchColor = new Color(0f, 192f / 255f, 0f, 1.0f);
        inacticveColor = new Color(128f / 255f, 128f / 255f, 128f / 255f, 1.0f);
    }

    public void SetupTerrain(MapBoard mapcontroller_, TerrainType type_, int tileIdx1D_)
    {
        terrainType = type_;
        mapController = mapcontroller_;
        TileIdx1D = tileIdx1D_;

        TileIdx2D = mapController.GetTileIndex2D(TileIdx1D);

        string prefabName = string.Empty;
        switch (type_)
        {
            case TerrainType.PLAIN_PASS:
                prefabName = RDefine.TERRAIN_PREF_PLAIN;
                IsPassable = true;
                break;
            case TerrainType.OCEAN_N_PASS:
                prefabName = RDefine.TERRAIN_PREF_OCEAN;
                IsPassable = false;
                break;
            default:
                prefabName = "Tile_Default";
                IsPassable = false;
                break;
        }
        this.name = string.Format("{0}_{1}", prefabName, TileIdx1D);

    }

    public void SetTileActiveColor(RDefine.TileStatusColor tileStatus)
    {
        switch (tileStatus)
        {
            case RDefine.TileStatusColor.Selected:
                frontRenderer.color = selectedColor;
                break;
            case RDefine.TileStatusColor.Search:
                frontRenderer.color = searchColor;
                break;
            case RDefine.TileStatusColor.Inactive:
                frontRenderer.color = inacticveColor;
                break;
            default:
                frontRenderer.color = defaultColor;
                break;
        }
    }
}
