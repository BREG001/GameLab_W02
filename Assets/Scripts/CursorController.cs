using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private CursorEventArgs cursorEvent;

    [SerializeField] private SpriteRenderer cursorSprite;

    [SerializeField] private Vector2Int _startTilePoint;
    [SerializeField] private Vector2Int _endTilePoint;

    void Awake()
    {
        
    }

    void Start()
    {
        _startTilePoint = FarmController.instance.StartTilePos;
        _endTilePoint = _startTilePoint +
                        new Vector2Int(FarmController.instance._width - 1,
                            FarmController.instance._height - 1);

        cursorEvent.CursorClick += OnCursorClick;
    }

    void OnDestroy()
    {
        cursorEvent.CursorClick -= OnCursorClick;
    }

    void Update()
    {
        if (!CheckCursorOutOfTilemap())
        {
            DrawCursor();

            if (Input.GetMouseButtonDown(0))
                cursorEvent.CallCursorClick();
        }
        else
        {
            cursorSprite.enabled = false;
        }
    }

    private void DrawCursor()
    {
        if (!cursorSprite.enabled) cursorSprite.enabled = true;

        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos = new Vector2(Mathf.Round(_mousePos.x + 0.5f) - 0.5f,
                Mathf.Round(_mousePos.y + 0.5f) - 0.5f);

        transform.position = _mousePos;
    }

    private void OnCursorClick(CursorEventArgs cursorEvent)
    {
        PlantSeed(0);
    }

    public Vector3Int GetCursorTile()
    {
        // 커서가 클릭한 곳의 좌표를 가져온다
        return GameManager.Instance.MapGrid.WorldToCell(
            Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private bool CheckCursorOutOfTilemap()
    {
        Vector3Int cursorTile = GetCursorTile();
        if (cursorTile.x < _startTilePoint.x || cursorTile.x > _endTilePoint.x
            || cursorTile.y < _startTilePoint.y || cursorTile.y > _endTilePoint.y)
            return true;
        return false;
    }

    private void PlantSeed(int cropID)
    {
        if (CheckCursorOutOfTilemap()) return;

        Vector3 _cursorTile = GetCursorTile();
        Vector3Int plantPosition = new Vector3Int((int)_cursorTile.x, (int)_cursorTile.y);

        FarmController.instance.SetCrop(plantPosition.x, plantPosition.y, cropID);
    }
}
