using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmController : MonoBehaviour
{
    public static FarmController instance;

    public Crop[,] Crops;
    public int _width;
    public int _height;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitCrops(_width, _height);
    }

    private void InitCrops(int width, int height)
    {
        _width = width;
        _height = height;
        Crops = new Crop[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Crops[x, y] = new Crop();
                Crops[x, y].cropID = 0;
                Crops[x, y].growLevel = 0;
                Crops[x, y].growTime = 0f;
            }
        }
    }

    public void GrowCrop(int _x, int _y, int _id, int _level, float _time)
    {
        if (_id != 0)
        {
            Crops[_x, _y].cropID = _id;
            Crops[_x, _y].growLevel = _level;
            Crops[_x, _y].growTime = _time;
        }   
    }

    [Serializable]
    public class Crop
    {
        public int cropID;      // 농작물 id 값
        public int growLevel;   // 농작물 성장 단계(스프라이트 변경을 위하여)
        public float growTime;  // 경작 완료 시간
    }
}
