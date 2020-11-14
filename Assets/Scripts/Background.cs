using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Background : MonoBehaviour
{
    [SerializeField]
    [Range(.01f, 1f)]
    private float scrollSpeed = .1f;

    private float camLeftEdgePosX;
    private float _backgroundEndPosX;
    private float _backgroundImageWidth;

    public float BackgroundImageWidth => _backgroundImageWidth;

    private bool _isScrolling = false;
    public bool IsScrolling
    {
        get => _isScrolling;
        set => _isScrolling = value;
    }

    private void Awake()
    {
        PixelPerfectCamera cam = Camera.main.GetComponent<PixelPerfectCamera>();
        float camWidth = cam.refResolutionX / cam.assetsPPU;
        
        //set background position according to camera
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        camLeftEdgePosX = (camWidth / 2) - Camera.main.transform.position.x;
        _backgroundImageWidth = spriteRenderer.bounds.size.x;
        float backgroundStartPosX = -camLeftEdgePosX + (_backgroundImageWidth / 2);

        Debug.Log(_backgroundImageWidth);
        Debug.Log(backgroundStartPosX);
        transform.position = new Vector2(backgroundStartPosX - 1, 0f);
        
        //calculate position where camera starts to lose background
        float rightEdgePosX = (camWidth / 2) + Camera.main.transform.position.x;
        _backgroundEndPosX = rightEdgePosX - (_backgroundImageWidth / 2);
    }

    void Update()
    {
        if (_isScrolling)
        {
            Vector3 pos = transform.position;
            pos.x -= scrollSpeed;
            transform.position = pos;
        }
    }
    
    public bool isNearEnd()
    {
        return transform.position.x - _backgroundEndPosX < 1;
    }

    public bool isOutOfCamFocus()
    {
        return transform.position.x + (_backgroundImageWidth / 2) < camLeftEdgePosX;
    }

}
