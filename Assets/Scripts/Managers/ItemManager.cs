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
        // �ش� �۹� �� �߰�
        cropCount[_cropId]++;
        // �ش� �۹� ���� �߰�
        money += GameManager.Instance.Crops[_cropId].SellMoney;

        // UI ������Ʈ
        GameManager.Instance.GameUI.CropCountUI.CropCountUpdate(_cropId);
        GameManager.Instance.GameUI.SetMoneyText();

        Debug.Log($"{GameManager.Instance.Crops[_cropId].CropName}({_cropId}): {cropCount[_cropId]}");
    }

    public bool UseMoney(int _upgradeId)
    {
        // ���׷��̵� ������ ���� ���

        // ���� �������� ��뺸�� ������
        if (money < GameManager.Instance.Upgrades[_upgradeId].Money[0])
        {
            Debug.Log($"There is not enough money to purchase {GameManager.Instance.Upgrades[_upgradeId].UpgradeName}. {money} < {GameManager.Instance.Upgrades[_upgradeId].Money}");
            return false;
        }
        // ���� �����ϸ�
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
        // ���� ���ŷ� �� ���

        // ���� �������� ��뺸�� ������ false return
        if (money < GameManager.Instance.Crops[_cropId].PurchaseMoney)
        {
            Debug.Log($"There is not enough money to purchase {GameManager.Instance.Crops[_cropId].CropName}. {money} < {GameManager.Instance.Crops[_cropId].PurchaseMoney}");
            return false;
        }
        // ���� �����ϸ� �ݾ� ���� �� true return
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
        // �̹� �ر��� �����̸� false
        if (unlockedCrop[_cropId]) return false;

        if (GameManager.Instance.Crops[_cropId].UnlockCrops.Length > 0)
        {
            // �رݿ� �۹��� �ʿ��ϴٸ�

            int _id = 0;
            int _needCount = 0;

            for (int i = 0; i < GameManager.Instance.Crops[_cropId].UnlockCrops.Length; i++)
            {
                _id = GameManager.Instance.Crops[_cropId].UnlockCrops[i].Id;
                _needCount = GameManager.Instance.Crops[_cropId].UnlockCrops[i].Count;

                // �ش� �۹��� ����
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
            // �رݿ� �۹��� �ʿ���ٸ�
            Debug.Log($"No resources are needed for the cancellation of {GameManager.Instance.Crops[_cropId].CropName}.");
            return true;
        }
    }

    public void UnlockCrop(int _cropId)
    {
        unlockedCrop[_cropId] = true;
    }
}