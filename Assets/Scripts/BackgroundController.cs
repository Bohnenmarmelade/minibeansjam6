using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    [Range(.01f, 1f)]
    private float scrollSpeed = .1f;

    private float _backgroundEndPosX;
    
    private bool _isScrolling = false;
    public bool IsScrolling
    {
        get => _isScrolling;
        set => _isScrolling = value;
    }

    private void Awake()
    {
        //set background position according to camera
        PixelPerfectCamera cam = Camera.main.GetComponent<PixelPerfectCamera>();
        float camWidth = cam.refResolutionX / cam.assetsPPU;
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float leftEdgePosX = (camWidth / 2) - Camera.main.transform.position.x;
        float backgroundImageWidth = spriteRenderer.bounds.size.x;
        float backgroundStartPosX = -leftEdgePosX + (backgroundImageWidth / 2);

        Debug.Log(backgroundImageWidth);
        Debug.Log(backgroundStartPosX);
        transform.position = new Vector2(backgroundStartPosX - 1, 0f);
        
        //calculate position where camera starts to lose background
        
        float rightEdgePosX = (camWidth / 2) + Camera.main.transform.position.x;
        _backgroundEndPosX = rightEdgePosX - (backgroundImageWidth / 2);
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


}
