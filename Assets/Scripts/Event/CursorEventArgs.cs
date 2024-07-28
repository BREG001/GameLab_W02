using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEventArgs : MonoBehaviour
{
    public Action<CursorEventArgs> CursorClick;

    public void CallCursorClick()
    {
        CursorClick?.Invoke(this);
    }
}
