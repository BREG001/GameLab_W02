using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;       // ������ �ؽ�Ʈ
    public TextMeshProUGUI DescriptionText; // ������ ���� �ؽ�Ʈ
    public CropCountUI CropCountUI;

    public GameObject ExpandedUI;

    public bool isUIExpanded = true;

    void Start()
    {
        LoadAll();
    }

    public void LoadAll()
    {
        SetMoneyText();
        SetDescText("");
    }

    public void SetMoneyText()
    {
        MoneyText.text = ItemManager.Instance.Money.ToString();
    }

    public void SetDescText(string _desc)
    {
        DescriptionText.text = _desc;
    }

    public void ExpandUI()
    {
        if (isUIExpanded)
        {
            isUIExpanded = false;
        }
        else
        {
            isUIExpanded = true;
        }
        ExpandedUI.SetActive(isUIExpanded);
    }
}
