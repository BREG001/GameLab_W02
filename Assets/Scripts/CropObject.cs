using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropObject : MonoBehaviour
{
    [SerializeField] private GameObject[] Sprites;

    void Awake()
    {
        Sprites[0].SetActive(true);
        Sprites[1].SetActive(false);
        Sprites[2].SetActive(false);
        Sprites[3].SetActive(false);
    }

    public void ChangeSprite(int level)
    {
        for (int i = 0; i < Sprites.Length; i++)
        {
            if (i == level - 1)
                Sprites[i].SetActive(true);
            else
                Sprites[i].SetActive(false);
        }
    }
}
