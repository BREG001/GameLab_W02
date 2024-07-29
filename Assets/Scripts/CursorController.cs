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
            // 커서가 타일맵 위에 있을 시 커서 출력 및 클릭 이벤트 추가
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
            // 커서가 타일맵 위에 없으면 커서 출력 X
            cursorSprite.enabled = false;
        }
    }

    private void DrawCursor()
    {
        // 커서 출력
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
        // 커서가 클릭한 곳의 좌표를 가져온다
        return GameManager.Instance.MapGrid.WorldToCell(
            Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private bool CheckCursorOutOfTilemap()
    {
        // 커서가 타입맵위에 없는지 확인
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
