using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CropButton : MonoBehaviour
{
    [SerializeField] private int _cropID;
    [SerializeField] private bool _isUnlocked;

    [SerializeField] private GameObject LockedPanel;
    [SerializeField] private TextMeshProUGUI CropNameText;

    void Start()
    {
        _isUnlocked = ItemManager.Instance.unlockedCrop[_cropID];
        LockedPanel.SetActive(!_isUnlocked);
        CropNameText.text = GameManager.Instance.Crops[_cropID].CropName;
    }

    public void ClickButton()
    {
        GameManager.Instance.GameUI.SetDescText(GameManager.Instance.Crops[_cropID].Desc);
        if (_isUnlocked)
        {
            GameManager.Instance.Cursor.SetCursorPlantId(_cropID);
            Debug.Log($"Set cursor mode: Plant {GameManager.Instance.Crops[_cropID].CropName}");
        }
        else
        {
            if (ItemManager.Instance.CheckCanUnlockCrop(_cropID))
            {
                ItemManager.Instance.UnlockCrop(_cropID);
                _isUnlocked = true;
                LockedPanel.SetActive(false);
            }
        }
    }
}
