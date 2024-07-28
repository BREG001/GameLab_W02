using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Update()
    {
        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _mousePos = new Vector2(Mathf.Round(_mousePos.x + 0.5f) - 0.5f, 
                Mathf.Round(_mousePos.y + 0.5f) - 0.5f);

        transform.position = _mousePos;

        if (Input.GetMouseButtonDown(0))
            Debug.Log(GetCursorTile());
    }

    public Vector3Int GetCursorTile()
    {
        // 커서가 클릭한 곳의 좌표를 가져온다
        return GameManager.Instance.MapGrid.WorldToCell(
            Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
