using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Scriptable Object/Crop Data")]
public class CropData : ScriptableObject
{
    public string CropName;
    public int CropID;
    /*
        -2 : 벽
        -1 : 공백
        0~ : 작물
    */
    [Tooltip("성장 시간. 초 단위")]
    public float GrowTime;
    public GameObject CropPrefab;
    public int PurchaseMoney;
    public int SellMoney;
    [Multiline (5)]
    public string Desc;
    public string GrowTimeDesc;
}
