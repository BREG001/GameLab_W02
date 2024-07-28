using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _tf;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float moveSpeed = 0f;

    private Vector2 moveVector = Vector2.zero;

    void Start()
    {
        
    }

    void Update()
    {
        GetKeyInput();
        if (Input.GetKeyDown(KeyCode.Space))
            GetPlayerTile();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        _rb.velocity = moveVector * moveSpeed;
    }

    void GetKeyInput()
    {
        moveVector = Vector2.right * Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");
    }

    public void GetPlayerTile()
    {
        // 임시로 현재 플레이어가 위치한 자리의 타일 좌표를 가져옴
        Debug.Log(GameManager.Instance.MapGrid.WorldToCell(_tf.position));
    }
}
