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
        // 커서가 타일맵 위에 있을 시 커서 출력 및 클릭 이벤트 추가
        if (!CheckCursorOutOfTilemap())
        {
            // 커서 출력
            DrawCursor();

            // 모드가 없을 떄 수확가능한 타일을 클릭하면 수확 모드로 설정
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
            // 우클릭 시 심기 모드 취소
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
        // 커서가 타일맵 위에 없으면 커서 출력 X
        else
        {
            cursorSprite.enabled = false;
            if (seedCursorContainer[_cursorCropId].activeSelf)
                seedCursorContainer[_cursorCropId].SetActive(false);
        }

        // 우클릭 시 심기 모드 취소
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
        // 커서 출력
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
        Plant,
        Water
    }
}
