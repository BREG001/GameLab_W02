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
        -2 : ��
        -1 : ����
        0~ : �۹�
    */
    [Tooltip("���� �ð�. �� ����")]
    public float GrowTime;
    public GameObject CropObject;
}
