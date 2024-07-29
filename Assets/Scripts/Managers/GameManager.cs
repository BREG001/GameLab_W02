using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameUI GameUI;
    public Grid MapGrid;
    public CursorController Cursor;

    [Header("Resources")]
    public CropData[] Crops;
    public UpgradeData[] Upgrades;

    void Awake()
    {
        if (null == instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }
}