using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int points = 0;

    public void AddPoint()
    {
        points++;
    }

    public int GetPoints()
    {
        return points;
    }

}
