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
    [SerializeField]
    public Dictionary<int, int> cropCount = new Dictionary<int, int>();

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
        // Dict�� �ش��ϴ� �۹��� id�� ������ �߰�
        if (!cropCount.ContainsKey(_cropId))
            cropCount.Add(_cropId, 0);

        // �ش� �۹� �� �߰�
        cropCount[_cropId]++;
        // �ش� �۹� ���� �߰�
        money += GameManager.Instance.Crops[_cropId].SellMoney;

        Debug.Log($"{GameManager.Instance.Crops[_cropId].CropName}({_cropId}): {cropCount[_cropId]}");
    }

    public bool UseMoney(int _upgradeId)
    {
        // ���׷��̵� ������ ���� ���

        // ���� �������� ��뺸�� ������
        if (money < GameManager.Instance.Upgrades[_upgradeId].Money)
        {
            Debug.Log($"There is not enough money to purchase {GameManager.Instance.Upgrades[_upgradeId].UpgradeName}. {money} < {GameManager.Instance.Upgrades[_upgradeId].Money}");
            return false;
        }
        // ���� �����ϸ�
        else
        {
            money -= GameManager.Instance.Upgrades[_upgradeId].Money;
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
            Debug.Log($"Plant {GameManager.Instance.Crops[_cropId].CropName}");
            return true;
        }
    }
}
