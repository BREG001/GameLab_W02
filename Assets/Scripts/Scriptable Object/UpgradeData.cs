using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Scriptable Object/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string UpgradeName;
    public int UpgradeId;
    public int Money;
    public GameObject UpgradePrefab;

    public enum UpgradeType
    {
        increaseFarm
    }
}
