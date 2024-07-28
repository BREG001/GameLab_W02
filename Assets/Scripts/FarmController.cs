using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmController : MonoBehaviour
{
    public static FarmController instance;

    public Crop[,] Crops;
    public Vector2Int StartTilePos;
    public int _width;
    public int _height;

    public int cropsMaxLevel = 4;

    private CropData[] cropDatas;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitCrops(_width, _height);

        cropDatas = new CropData[GameManager.Instance.Crops.Length];
        for (int i = 0; i < cropDatas.Length; i++)
            cropDatas[i] = GameManager.Instance.Crops[i];
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
                Crops[x, y].cropID = -1;
                Crops[x, y].growLevel = 0;
                Crops[x, y].plantTime = 0f;
                Crops[x, y].cropObject = null;
            }
        }
    }

    void Update()
    {
        CheckCropState();
    }

    private void CheckCropState()
    {
        // 모든 타일의 작물 상태 확인, 추후 개방한 구역만 확인하도록 줄일 예정
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // 해당 타일에 아무것도 없으면 다음
                if (Crops[x, y].cropID == -1)
                    continue;

                // 해당 작물 경과 시간
                float _elapsedTime = Time.realtimeSinceStartup - Crops[x, y].plantTime;
                // 해당 작물 다음 성장까지 남은 시간
                float _currentGrowTime = cropDatas[Crops[x, y].cropID].GrowTime * ((float)Crops[x, y].growLevel / 3f);

                // 다음 성장 단계 충족 시 && 성장 완료가 아닐 시
                if (_elapsedTime > _currentGrowTime && Crops[x, y].growLevel < cropsMaxLevel)
                {
                    Crops[x, y].growLevel++;
                    Debug.Log($"[{x}, {y}] => {Crops[x, y].growLevel}");
                }
            }
        }
    }

    public void SetCrop(int _x, int _y, int _id, int _level)
    {
        int x = _x - StartTilePos.x;
        int y = _y - StartTilePos.y;

        if (Crops[x, y].cropID >= 0) 
        {
            Debug.Log($"Already planted {Crops[x, y].cropID} in [{x}, {y}], {Crops[x, y].plantTime}");
            return;
        }
        else if (Crops[x, y].cropID == -1)
        {
            Crops[x, y].cropID = _id;
            Crops[x, y].growLevel = _level;
            Crops[x, y].plantTime = Time.realtimeSinceStartup;
            Debug.Log($"Plant {_id} in [{x}, {y}], {Crops[x, y].plantTime}");
        }
        else
        {
            Debug.Log($"err: {_id} in [{x}, {y}], {Crops[x, y].plantTime}");
        }
    }

    public void SetCrop(int _x, int _y, int _id)
    {
        SetCrop(_x, _y, _id, 1);
    }

    [Serializable]
    public class Crop
    {
        public int cropID;              // 농작물 id 값
        public int growLevel;           // 농작물 성장 단계(스프라이트 변경을 위하여)
        public float plantTime;         // 심은 시간
        public GameObject cropObject;   // 작물 오브젝트
    }
}
