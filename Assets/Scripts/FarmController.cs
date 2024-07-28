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
    [SerializeField] private Transform cropsParentTf;

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
                Crops[x, y].isGrown = false;
                Crops[x, y].growLevel = 0;
                Crops[x, y].plantTime = 0f;
                Crops[x, y].realGameObject = null;
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
                // �ش� Ÿ�Ͽ� �ƹ��͵� ���ų� �̹� �� �ڶ��ٸ� ���� Ÿ�Ϸ�
                if (Crops[x, y].cropID == -1 || Crops[x, y].isGrown)
                    continue;

                // _elapsedTime: �ش� �۹� ��� �ð�
                // _currentGrowTime: �ش� �۹� ���� ������� ���� �ð�
                float _elapsedTime = Time.realtimeSinceStartup - Crops[x, y].plantTime;
                float _currentGrowTime =
                    cropDatas[Crops[x, y].cropID].GrowTime * 
                    Mathf.Min(((float)Crops[x, y].growLevel / 3f), 1f);

                // ���� ���� �ܰ� ���� ��
                if (_elapsedTime > _currentGrowTime)
                {
                    // ���� ���� X
                    if (Crops[x, y].growLevel < cropsMaxLevel)
                    {
                        // �۹� ���� �ܰ�� ���� �� ��������Ʈ ����
                        Crops[x, y].cropObject.ChangeSprite(++Crops[x, y].growLevel);
                    }
                    // ���� ���� �� ���� �Ϸ� true
                    else if (Crops[x, y].growLevel == cropsMaxLevel)
                    {
                        Crops[x, y].isGrown = true;
                    }
                }
            }
        }
    }

    public void HarvestCrop(int _x, int _y)
    {
        int x = _x - StartTilePos.x;
        int y = _y - StartTilePos.y;

        if (!Crops[x, y].isGrown)
            return;

        Destroy(Crops[x, y].realGameObject);

        Crops[x, y].cropID = -1;
        Crops[x, y].isGrown = false;
        Crops[x, y].growLevel = 0;
        Crops[x, y].plantTime = 0f;
        Crops[x, y].realGameObject = null;
        Crops[x, y].cropObject = null;
    }

    public void PlantCrop(int _x, int _y, int _id)
    {
        int x = _x - StartTilePos.x;
        int y = _y - StartTilePos.y;

        if (Crops[x, y].cropID >= 0) 
        {
            // �̹� �ɾ����ִ� �Ĺ��� ������ return
            Debug.Log($"Already planted {Crops[x, y].cropID} in [{x}, {y}], {Crops[x, y].plantTime}");
            return;
        }
        else if (Crops[x, y].cropID == -1)
        {
            // �� Ÿ���̶�� _id�� �Ĺ� ����
            Crops[x, y].cropID = _id;
            Crops[x, y].isGrown = false;
            Crops[x, y].growLevel = 1;
            Crops[x, y].plantTime = Time.realtimeSinceStartup;

            Crops[x, y].realGameObject = Instantiate(cropDatas[_id].CropObject, cropsParentTf);
            Crops[x, y].realGameObject.transform.position = new Vector3(_x + 0.5f, _y + 0.5f, 0);

            Crops[x, y].cropObject = Crops[x, y].realGameObject.GetComponent<CropObject>();
            Debug.Log($"Plant {_id} in [{x}, {y}], {Crops[x, y].plantTime}");
        }
        else
        {
            // Ȯ�ε��� ���� ����� ��
            Debug.Log($"err: {_id} in [{x}, {y}], {Crops[x, y].plantTime}");
        }
    }

    [Serializable]
    public class Crop
    {
        public int cropID;                  // ���۹� id ��
        public bool isGrown;                 // ���� �Ϸ� ����
        public int growLevel;               // ���۹� ���� �ܰ�(��������Ʈ ������ ���Ͽ�)
        public float plantTime;             // ���� �ð�
        public GameObject realGameObject;   // �۹� ������Ʈ
        public CropObject cropObject;       // �۹� ��������Ʈ ���� ������Ʈ
    }
}
