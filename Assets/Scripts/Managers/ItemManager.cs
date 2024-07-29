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
        // Dict에 해당하는 작물의 id가 없으면 추가
        if (!cropCount.ContainsKey(_cropId))
            cropCount.Add(_cropId, 0);

        // 해당 작물 수 추가
        cropCount[_cropId]++;
        // 해당 작물 가격 추가
        money += GameManager.Instance.Crops[_cropId].SellMoney;

        Debug.Log($"{GameManager.Instance.Crops[_cropId].CropName}({_cropId}): {cropCount[_cropId]}");
    }

    public bool UseMoney(int _upgradeId)
    {
        // 업그레이드 등으로 돈을 사용

        // 현재 소지금이 비용보다 낮으면
        if (money < GameManager.Instance.Upgrades[_upgradeId].Money)
        {
            Debug.Log($"There is not enough money to purchase {GameManager.Instance.Upgrades[_upgradeId].UpgradeName}. {money} < {GameManager.Instance.Upgrades[_upgradeId].Money}");
            return false;
        }
        // 구매 가능하면
        else
        {
            money -= GameManager.Instance.Upgrades[_upgradeId].Money;
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
            Debug.Log($"Plant {GameManager.Instance.Crops[_cropId].CropName}");
            return true;
        }
    }
}
