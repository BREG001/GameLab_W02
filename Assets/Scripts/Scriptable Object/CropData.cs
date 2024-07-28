using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Scriptable Object/Crop Data")]
public class CropData : ScriptableObject
{
    public string CropName;
    public int CropID;
    [Tooltip("성장 시간. 초 단위")]
    public float GrowTime;
}
