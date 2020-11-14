using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.XR.WSA;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 100f)]
    private float maxMovementSpeed = 1f;
    [SerializeField]
    [Range(1f, 100f)]
    private float accelerationMultiplier = 20f;
    
    private float _boundaryMargin = .5f;
    
    private Vector2 _movement = Vector2.zero;
    private float _horizontalMove;
    private float _verticalMove;

    private Camera _mainCam;
    private float _camWidth;
    private float _camHeight; 
    
    
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _mainCam = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        PixelPerfectCamera cam = _mainCam.GetComponent<PixelPerfectCamera>();
        
        _camWidth = (float)cam.refResolutionX / cam.assetsPPU;
        _camHeight = (float)cam.refResolutionY / cam.assetsPPU;
    }

    void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        Move(_movement);
        CheckBoundaries();
    }

    private void CheckBoundaries()
    {
        //TODO: this is bad
        Vector3 pos = transform.position;
        Vector3 camPos = _mainCam.transform.position;
        float minX = camPos.x - _camWidth * .5f + _boundaryMargin;
        float maxX = camPos.x + _camWidth * .5f - _boundaryMargin;
        float minY = camPos.y - _camHeight * .5f + _boundaryMargin;
        float maxY = camPos.y + _camHeight * .5f - _boundaryMargin;
        if (pos.x < minX)
        {
            pos.x = minX;
        } else if (pos.x > maxX)
        {
            pos.x = maxX;
        }

        if (pos.y < minY)
        {
            pos.y = minY;
        } else if (pos.y > maxY)
        {
            pos.y = maxY;
        }

        transform.position = pos;
    }

    private void Move(Vector2 movement)
    {
        _rigidbody2D.AddForce(movement * accelerationMultiplier);
        _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, maxMovementSpeed);
        
    }
}
