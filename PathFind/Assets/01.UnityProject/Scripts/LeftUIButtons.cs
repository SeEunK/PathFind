using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftUIButtons : MonoBehaviour
{
  //A start find path 누른 경우

    public void OnClickAstarFindBtn()
    {
        PathFinder.Instance.FindPathAstar();
    }

}
