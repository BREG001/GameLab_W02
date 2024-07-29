using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CropCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] countTexts;

    void Start()
    {
        LoadAllCropCount();
    }

    public void LoadAllCropCount()
    {
        for (int i = 0; i < GameManager.Instance.Crops.Length; i++)
        {
            countTexts[i].text = ItemManager.Instance.cropCount[i].ToString();
        }
    }

    public void CropCountUpdate(int _cropId)
    {
        countTexts[_cropId].text = ItemManager.Instance.cropCount[_cropId].ToString();
    }
}
