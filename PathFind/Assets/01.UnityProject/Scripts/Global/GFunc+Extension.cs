using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GFunc
{
    //딕셔너리에 오브젝트 프리팹을 캐싱하는 함수
    public static void AddObjs(this Dictionary<string, GameObject> dict_, GameObject[] prefabs_)
    {
        foreach(var prefab_ in prefabs_)
        {
            dict_.Add(prefab_.name, prefab_);
        }
    } //AddObjs()


    //!float 
    public static bool IsEquals(this float targetValue, float compareValue)
    {
        bool isEquals = Mathf.Approximately(targetValue, compareValue);
        return isEquals;
    }

    // 리트스 섞는 함수 
    public static void Shuffle<T>(this List<T> targetList, int shuffleCnt = 0)
    {
        if (shuffleCnt.Equals(0))
        {
            shuffleCnt = (int)(targetList.Count * 2.0f);
        }
        int sourIndex = 0;
        int destIndex = 0;

        T tempVar = default(T);

        for (int i = 0; i < shuffleCnt; i++)
        {
            sourIndex = Random.Range(0, targetList.Count);
            destIndex = Random.Range(0, targetList.Count);

            tempVar = targetList[sourIndex];
            targetList[sourIndex] = targetList[destIndex];
            targetList[destIndex] = tempVar;
        }

    }

    // 리스트의 element 를 다른 값과 swap 하는 함수
    public static void Swap<T>(this List<T> targetList, ref T swapValue, int swapIndex)
    {
        T tempValue = targetList[swapIndex];
        targetList[swapIndex] = swapValue;
        swapValue = tempValue;
    }

    //int의 값이 범위안에 속해있는지 검사하는 함수
    public static bool IsInRange(this int targetValue, int minInclude, int maxInclude)
    {
        return (minInclude <= targetValue && targetValue < maxInclude);
    }


    #region AstarFunction
    public static AstarNode FindNode(this List<AstarNode> nodeList, AstarNode compareNode)
    {
        if (nodeList.IsValid() == false) { return default; }

        AstarNode resultNode = default;
        foreach (var node_ in nodeList)
        {
            if (node_.terrain == default || node_.terrain == null) { continue; }
            else if (compareNode.terrain == default || compareNode.terrain == null) { continue; }

            if (node_.terrain.TileIdx1D.Equals(compareNode.terrain.TileIdx1D))
            {
                resultNode = node_;
            }
            else { continue; }
        }
        return resultNode;

    }
    #endregion
}
