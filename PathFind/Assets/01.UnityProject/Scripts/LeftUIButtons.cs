using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftUIButtons : MonoBehaviour
{
  //A start find path ���� ���

    public void OnClickAstarFindBtn()
    {
        PathFinder.Instance.FindPathAstar();
    }

}
