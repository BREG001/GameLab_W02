using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance = null;

    [SerializeField]
    private int money;
    public int Money { get { return money; } }
    public int[] cropCount = new int[12];
    public bool[] unlockedCrop = new bool[12];

    void Awake()
    {
        if (null == instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void HarvestCrop(int _cropId)
    {
        // 해당 작물 수 추가
        cropCount[_cropId]++;
        // 해당 작물 가격 추가
        money += GameManager.Instance.Crops[_cropId].SellMoney;

        // UI 업데이트
        GameManager.Instance.GameUI.CropCountUI.CropCountUpdate(_cropId);
        GameManager.Instance.GameUI.SetMoneyText();

        Debug.Log($"{GameManager.Instance.Crops[_cropId].CropName}({_cropId}): {cropCount[_cropId]}");
    }

    public bool UseMoney(int _upgradeId)
    {
        // 업그레이드 등으로 돈을 사용

        // 현재 소지금이 비용보다 낮으면
        if (money < GameManager.Instance.Upgrades[_upgradeId].Money[0])
        {
            Debug.Log($"There is not enough money to purchase {GameManager.Instance.Upgrades[_upgradeId].UpgradeName}. {money} < {GameManager.Instance.Upgrades[_upgradeId].Money}");
            return false;
        }
        // 구매 가능하면
        else
        {
            money -= GameManager.Instance.Upgrades[_upgradeId].Money[0];
            GameManager.Instance.GameUI.SetMoneyText();
            Debug.Log($"Purchase {GameManager.Instance.Upgrades[_upgradeId].UpgradeName}");
            return true;
        }
    }

    public bool PlantSeed(int _cropId)
    {
        // 씨앗 구매로 돈 사용

        // 현재 소지금이 비용보다 낮으면 false return
        if (money < GameManager.Instance.Crops[_cropId].PurchaseMoney)
        {
            Debug.Log($"There is not enough money to purchase {GameManager.Instance.Crops[_cropId].CropName}. {money} < {GameManager.Instance.Crops[_cropId].PurchaseMoney}");
            return false;
        }
        // 구매 가능하면 금액 차감 후 true return
        else
        {
            money -= GameManager.Instance.Crops[_cropId].PurchaseMoney;
            GameManager.Instance.GameUI.SetMoneyText();
            Debug.Log($"Plant {GameManager.Instance.Crops[_cropId].CropName}");
            return true;
        }
    }

    public bool CheckCanUnlockCrop(int _cropId)
    {
        // 이미 해금한 상태이면 false
        if (unlockedCrop[_cropId]) return false;

        if (GameManager.Instance.Crops[_cropId].UnlockCrops.Length > 0)
        {
            // 해금에 작물이 필요하다면

            int _id = 0;
            int _needCount = 0;

            for (int i = 0; i < GameManager.Instance.Crops[_cropId].UnlockCrops.Length; i++)
            {
                _id = GameManager.Instance.Crops[_cropId].UnlockCrops[i].Id;
                _needCount = GameManager.Instance.Crops[_cropId].UnlockCrops[i].Count;

                // 해당 작물이 부족
                if (cropCount[_id] < _needCount)
                {
                    return false;
                    Debug.Log($"Can't unlock the {GameManager.Instance.Crops[_cropId].CropName}");
                }
            }

            Debug.Log($"Unlock the {GameManager.Instance.Crops[_cropId].CropName}");
            return true;
        }
        else
        {
            // 해금에 작물이 필요없다면
            Debug.Log($"No resources are needed for the cancellation of {GameManager.Instance.Crops[_cropId].CropName}.");
            return true;
        }
    }

    public void UnlockCrop(int _cropId)
    {
        unlockedCrop[_cropId] = true;
    }
}