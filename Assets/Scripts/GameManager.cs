using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Grid MapGrid;
    public Camera MainCamera;

    void Awake()
    {
        Instance = this;
    }
}