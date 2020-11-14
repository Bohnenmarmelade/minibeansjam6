using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1f, 10f)]
    public float movementSpeed;

    private float _horizontalMove;
    private float _verticalMove;
    private Vector2 _velocity = Vector3.zero;
    
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * movementSpeed;
        _verticalMove = Input.GetAxisRaw("Vertical") * movementSpeed;
    }

    private void FixedUpdate()
    {
        move(_horizontalMove, _verticalMove);
    }

    private void move(float horizontal, float vertical)
    {
        if (horizontal == 0 & vertical == 0)
        {
            _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, Vector2.zero, ref _velocity, .5f);
        } else {
            Vector2 targetVelocity = new Vector2(horizontal + _rigidbody2D.velocity.x, vertical + _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, 1f);
        }
    }
}
