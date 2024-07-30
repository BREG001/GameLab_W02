using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private int _upgradeID;

    [SerializeField] private TextMeshProUGUI UpgradeNameText;
    [SerializeField] private TextMeshProUGUI PriceText;
    [SerializeField] private TextMeshProUGUI CurrentLevelText;

    void Start()
    {
        UpgradeNameText.text = GameManager.Instance.Upgrades[_upgradeID].UpgradeName;
        PriceText.text = string.Format("���� �ܰ�: {0} MEM", GameManager.Instance.Upgrades[_upgradeID].Money[0]); ;
        CurrentLevelText.text = "���� 0�ܰ�";
    }

    public void ClickButton()
    {
        ItemManager.Instance.UseMoney(_upgradeID);
    }

    public void UpdateInfo()
    {
        UpgradeNameText.text = GameManager.Instance.Upgrades[_upgradeID].UpgradeName;
        if (ItemManager.Instance.UpgradeLevels[_upgradeID] < GameManager.Instance.Upgrades[_upgradeID].MaxLevel)
            PriceText.text = string.Format("���� �ܰ�: {0} MEM", GameManager.Instance.Upgrades[_upgradeID].Money[ItemManager.Instance.UpgradeLevels[_upgradeID]]);
        else
        {
            PriceText.text = string.Format("���� �ܰ� ����");
        }
        CurrentLevelText.text = string.Format("���� {0}�ܰ�", ItemManager.Instance.UpgradeLevels[_upgradeID]);
    }
}
