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
        // 모든 타일의 작물 상태 확인, 추후 개방한 구역만 확인하도록 줄일 예정
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // 해당 타일에 아무것도 없거나 이미 다 자랐다면 다음 타일로
                if (Crops[x, y].cropID == -1 || Crops[x, y].isGrown)
                    continue;

                // _elapsedTime: 해당 작물 경과 시간
                // _currentGrowTime: 해당 작물 다음 성장까지 남은 시간
                float _elapsedTime = Time.realtimeSinceStartup - Crops[x, y].plantTime;
                float _currentGrowTime =
                    cropDatas[Crops[x, y].cropID].GrowTime * 
                    Mathf.Min(((float)Crops[x, y].growLevel / 3f), 1f);

                // 다음 성장 단계 충족 시
                if (_elapsedTime > _currentGrowTime)
                {
                    // 성장 만렙 X
                    if (Crops[x, y].growLevel < cropsMaxLevel)
                    {
                        // 작물 다음 단계로 성장 및 스프라이트 변경
                        Crops[x, y].cropObject.ChangeSprite(++Crops[x, y].growLevel);
                    }
                    // 성장 만렙 시 성장 완료 true
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
            // 이미 심어져있는 식물이 있으면 return
            Debug.Log($"Already planted {Crops[x, y].cropID} in [{x}, {y}], {Crops[x, y].plantTime}");
            return;
        }
        else if (Crops[x, y].cropID == -1)
        {
            // 빈 타일이라면 _id의 식물 적용
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
            // 확인되지 않은 경우의 수
            Debug.Log($"err: {_id} in [{x}, {y}], {Crops[x, y].plantTime}");
        }
    }

    [Serializable]
    public class Crop
    {
        public int cropID;                  // 농작물 id 값
        public bool isGrown;                 // 성장 완료 여부
        public int growLevel;               // 농작물 성장 단계(스프라이트 변경을 위하여)
        public float plantTime;             // 심은 시간
        public GameObject realGameObject;   // 작물 오브젝트
        public CropObject cropObject;       // 작물 스프라이트 관리 컴포넌트
    }
}
