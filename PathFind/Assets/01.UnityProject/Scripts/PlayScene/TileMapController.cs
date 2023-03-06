using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileMapController : MonoBehaviour
{
    protected string tileMapObjName = default;

    protected MapBoard mapController = default;

    [SerializeField]
    protected Tilemap tileMap = default;
    [SerializeField]
    protected List<GameObject> allTileObjs = default;


    public virtual void InitAwake(MapBoard mapController_ )
    {
        mapController= mapController_;
        tileMap = gameObject.FindChildComponent<Tilemap>(tileMapObjName);

        //직사각형 형태로 초기화된 타일을 캐싱해서 가지고 있는다.
        allTileObjs = tileMap.gameObject.GetChildrenObjs();
        if (allTileObjs.IsValid())
        {
            allTileObjs.Sort(GFunc.CompareTileObjToLocalPos2D);
        }
        else
        {
            allTileObjs = new List<GameObject>();
        }
        /* ToDo */
    }

}
