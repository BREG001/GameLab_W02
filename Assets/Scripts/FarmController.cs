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
        // ��� Ÿ���� �۹� ���� Ȯ��, ���� ������ ������ Ȯ���ϵ��� ���� ����
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // �ش� Ÿ�Ͽ� �ƹ��͵� ������ ����
                if (Crops[x, y].cropID == -1)
                    continue;

                // �ش� �۹� ��� �ð�
                float _elapsedTime = Time.realtimeSinceStartup - Crops[x, y].plantTime;
                // �ش� �۹� ���� ������� ���� �ð�
                float _currentGrowTime = cropDatas[Crops[x, y].cropID].GrowTime * ((float)Crops[x, y].growLevel / 3f);

                // ���� ���� �ܰ� ���� �� && ���� �Ϸᰡ �ƴ� ��
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
        public int cropID;              // ���۹� id ��
        public int growLevel;           // ���۹� ���� �ܰ�(��������Ʈ ������ ���Ͽ�)
        public float plantTime;         // ���� �ð�
        public GameObject cropObject;   // �۹� ������Ʈ
    }
}
