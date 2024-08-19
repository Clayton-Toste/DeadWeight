using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    Texture2D texture;

    void Start()
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        Destroy(this);
    }
}
