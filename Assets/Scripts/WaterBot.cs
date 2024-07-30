using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBot : MonoBehaviour
{
    public Vector2Int Coordinate;
    public Vector2Int[] Direction;
    public float WaterCycleTime;
    public float LastWaterTime;

    void Start()
    {
        LastWaterTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if (Time.realtimeSinceStartup - LastWaterTime > WaterCycleTime)
        {
            for (int i = 0; i < Direction.Length; i++)
            {
                Vector2Int wateringCoor = Coordinate + Direction[i];
                //if ()
            }

            LastWaterTime = Time.realtimeSinceStartup;
        }
    }
}
