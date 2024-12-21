using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Serialisable fields
    [Header("Player Parameters")]
    [SerializeField] private float _initialGravityScale;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rayLength = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    // References
    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _collider;

    // Variables
    private float _horizontalInput;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _body.gravityScale = _initialGravityScale;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        /*
            MOVE CHARACTER LOGIC
        */
        // Move player and flip character
        FlipCharacter(_horizontalInput);

        // Adjustable jump
        if (Input.GetKeyUp(KeyCode.Space) && _body.linearVelocityY > 0)
        {
            _body.linearVelocity = new Vector2(_body.linearVelocityX, _body.linearVelocityY / 2);
        }

        if (onWall() && !isGrounded()){
            Debug.Log("I'm stuck in the wall");
            _body.linearVelocity = new Vector2(0, _body.linearVelocityY / 2);
        } else {
            _body.linearVelocity = new Vector2(_horizontalInput * _movementSpeed, _body.linearVelocityY);
        }

        HandleAnimation();

        /*
            JUMP LOGIC
        */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void HandleAnimation()
    {
        _animator.SetBool("running", _horizontalInput != 0);
        _animator.SetBool("grounded", isGrounded());
    }

    private void Jump()
    {
        _body.linearVelocity = new Vector2(_body.linearVelocityX, _jumpForce);
    }

    private void FlipCharacter(float direction)
    {
        if (direction > 0.01f)
            transform.localScale = Vector3.one;
        else if (direction < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    #region Detecting Ground
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, _rayLength, _groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, new Vector3(Mathf.Sign(transform.localScale.x), 0, 0), _rayLength, _groundLayer);
        return raycastHit.collider != null;
    }
    #endregion
}
