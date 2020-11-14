using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Background : MonoBehaviour
{
    private float _backgroundImageWidth;
    public float BackgroundImageWidth => _backgroundImageWidth;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _backgroundImageWidth = spriteRenderer.bounds.size.x;
    }

    public float GetRightEdgePosX()
    {
        return _backgroundImageWidth + transform.position.x;
    }


}
