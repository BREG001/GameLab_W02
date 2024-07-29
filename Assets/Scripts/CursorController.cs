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
        if (!CheckCursorOutOfTilemap())
        {
            // Ŀ���� Ÿ�ϸ� ���� ���� �� Ŀ�� ��� �� Ŭ�� �̺�Ʈ �߰�
            DrawCursor();

            if (Input.GetMouseButtonDown(0))
            {
                SetCursorMode();
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
                    FarmController.Instance.PlantCrop(plantPosition.x, plantPosition.y, 0);
                }
            }
        }
        else
        {
            // Ŀ���� Ÿ�ϸ� ���� ������ Ŀ�� ��� X
            cursorSprite.enabled = false;
        }
    }

    private void DrawCursor()
    {
        // Ŀ�� ���
        if (!cursorSprite.enabled) cursorSprite.enabled = true;

        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos = new Vector2(Mathf.Round(_mousePos.x + 0.5f) - 0.5f,
                Mathf.Round(_mousePos.y + 0.5f) - 0.5f);

        transform.position = _mousePos;
    }

    private void SetCursorMode()
    {
        Vector3 _cursorTile = GetCursorTile();
        Vector3Int plantPosition = new Vector3Int((int)_cursorTile.x, (int)_cursorTile.y);
        int x = plantPosition.x - _startTilePoint.x;
        int y = plantPosition.y - _startTilePoint.y;

        if (FarmController.Instance.Crops[x, y].isGrown)
            cursorMode = CursorModeEnum.Harvest;
        else if (!FarmController.Instance.Crops[x, y].isGrown)
            cursorMode = CursorModeEnum.Plant;
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
        Plant
    }
}
