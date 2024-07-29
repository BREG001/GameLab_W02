using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cursorSprite;

    [SerializeField] private Vector2Int _startTilePoint;
    [SerializeField] private Vector2Int _endTilePoint;

    public CursorModeEnum cursorMode = CursorModeEnum.None;
    [SerializeField] private int _cursorCropId;

    [SerializeField] private GameObject[] seedCursorContainer;

    void Awake()
    {
        
    }

    void Start()
    {
        _startTilePoint = FarmController.Instance.StartTilePos;
        _endTilePoint = _startTilePoint +
                        new Vector2Int(FarmController.Instance._width - 1,
                            FarmController.Instance._height - 1);
    }

    void Update()
    {
        // Ŀ���� Ÿ�ϸ� ���� ���� �� Ŀ�� ��� �� Ŭ�� �̺�Ʈ �߰�
        if (!CheckCursorOutOfTilemap())
        {
            // Ŀ�� ���
            DrawCursor();

            // ��尡 ���� �� ��Ȯ������ Ÿ���� Ŭ���ϸ� ��Ȯ ���� ����
            if (Input.GetMouseButtonDown(0))
            {
                if (cursorMode == CursorModeEnum.None)
                {
                    Vector3 _cursorTile = GetCursorTile();
                    Vector3Int plantPosition = new Vector3Int((int)_cursorTile.x, (int)_cursorTile.y);
                    int x = plantPosition.x - _startTilePoint.x;
                    int y = plantPosition.y - _startTilePoint.y;

                    if (FarmController.Instance.Crops[x, y].isGrown)
                        SetCursorMode(CursorModeEnum.Harvest);
                }
            }
            // ��Ŭ�� �� �ɱ� ��� ���
            else if (Input.GetMouseButtonDown(1))
            {
                if (cursorMode == CursorModeEnum.Plant || cursorMode == CursorModeEnum.Water)
                {
                    SetCursorMode(CursorModeEnum.None);
                    if (seedCursorContainer[_cursorCropId].activeSelf)
                        seedCursorContainer[_cursorCropId].SetActive(false);
                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 _cursorTile = GetCursorTile();
                Vector3Int plantPosition = new Vector3Int((int)_cursorTile.x, (int)_cursorTile.y);

                if (cursorMode == CursorModeEnum.Harvest)
                {
                    FarmController.Instance.HarvestCrop(plantPosition.x, plantPosition.y);
                }
                else if (cursorMode == CursorModeEnum.Plant)
                {
                    FarmController.Instance.PlantCrop(plantPosition.x, plantPosition.y, _cursorCropId);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (cursorMode == CursorModeEnum.Harvest)
                {
                    SetCursorMode(CursorModeEnum.None);
                }
            }
        }
        // Ŀ���� Ÿ�ϸ� ���� ������ Ŀ�� ��� X
        else
        {
            cursorSprite.enabled = false;
            if (seedCursorContainer[_cursorCropId].activeSelf)
                seedCursorContainer[_cursorCropId].SetActive(false);
        }

        // ��Ŭ�� �� �ɱ� ��� ���
        if (Input.GetMouseButtonDown(1))
        {
            if (cursorMode == CursorModeEnum.Plant)
            {
                SetCursorMode(CursorModeEnum.None);
                if (seedCursorContainer[_cursorCropId].activeSelf)
                    seedCursorContainer[_cursorCropId].SetActive(false);
            }
        }
    }

    public void SetCursorPlantId(int _cropId)
    {
        _cursorCropId = _cropId;
        SetCursorMode(CursorModeEnum.Plant);
    }

    private void DrawCursor()
    {
        // Ŀ�� ���
        if (cursorMode == CursorModeEnum.Plant)
        {
            if (cursorSprite.enabled)
            {
                for (int i = 0; i < seedCursorContainer.Length; i++)
                    seedCursorContainer[i].SetActive(i == _cursorCropId);
                cursorSprite.enabled = false;
            }
            else if (!seedCursorContainer[_cursorCropId].activeSelf)
            {
                for (int i = 0; i < seedCursorContainer.Length; i++)
                    seedCursorContainer[i].SetActive(i == _cursorCropId);
            }
        }
        else if (cursorMode != CursorModeEnum.Plant && !cursorSprite.enabled) 
            cursorSprite.enabled = true;

        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos = new Vector2(Mathf.Round(_mousePos.x + 0.5f) - 0.5f,
                Mathf.Round(_mousePos.y + 0.5f) - 0.5f);

        transform.position = _mousePos;
    }

    private void SetCursorMode(CursorModeEnum cModeEnum)
    {
        /*Vector3 _cursorTile = GetCursorTile();
        Vector3Int plantPosition = new Vector3Int((int)_cursorTile.x, (int)_cursorTile.y);
        int x = plantPosition.x - _startTilePoint.x;
        int y = plantPosition.y - _startTilePoint.y;

        if (FarmController.Instance.Crops[x, y].isGrown)
            cursorMode = CursorModeEnum.Harvest;
        else if (!FarmController.Instance.Crops[x, y].isGrown)
        {
            cursorMode = CursorModeEnum.Plant;
        }*/
        cursorMode = cModeEnum;
    }

    public Vector3Int GetCursorTile()
    {
        // Ŀ���� Ŭ���� ���� ��ǥ�� �����´�
        return GameManager.Instance.MapGrid.WorldToCell(
            Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private bool CheckCursorOutOfTilemap()
    {
        // Ŀ���� Ÿ�Ը����� ������ Ȯ��
        Vector3Int cursorTile = GetCursorTile();
        if (cursorTile.x < _startTilePoint.x || cursorTile.x > _endTilePoint.x
            || cursorTile.y < _startTilePoint.y || cursorTile.y > _endTilePoint.y)
            return true;
        return false;
    }

    public enum CursorModeEnum
    {
        None,
        Harvest,
        Plant,
        Water
    }
}
