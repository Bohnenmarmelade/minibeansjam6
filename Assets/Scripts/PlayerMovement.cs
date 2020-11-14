using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.XR.WSA;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 10f)]
    private float movementSpeed;

    private float _boundaryMargin = .5f;
    
    
    private float _horizontalMove;
    private float _verticalMove;
    private Vector2 _velocity = Vector3.zero;

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
        _horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;
        _verticalMove = Input.GetAxisRaw("Vertical") * movementSpeed;
    }

    private void FixedUpdate()
    {
        Move(_horizontalMove, _verticalMove);
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

    private void Move(float horizontal, float vertical)
    {
        if (horizontal == 0 & vertical == 0)
        {
            _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, Vector2.zero, ref _velocity, .2f);
        } else {
            Vector2 targetVelocity = new Vector2(horizontal + _rigidbody2D.velocity.x, vertical + _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, .05f);
        }
    }
}
