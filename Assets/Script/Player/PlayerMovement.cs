using UnityEngine;

// TODO: Add wall jump logic

public class PlayerMovement : MonoBehaviour
{
    // Serialisable fields
    [Header("Player Parameters")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _fallingSpeed;
    [SerializeField] private int _jumpCounterMax = 1;

    // References
    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _collider;

    // Variables
    private float _horizontalInput;
    private int _jumpCounter;
    private readonly float _rayLength = 0.01f;
    private float _initialGravityScale;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _initialGravityScale = _body.gravityScale;
        _fallingSpeed = _initialGravityScale * 1.5f;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        /*
            MOVE CHARACTER LOGIC
        */

        if (isGrounded()) Debug.Log("Grounded");

        // Move player and flip character
        FlipCharacter(_horizontalInput);
        if (!onWall() || isGrounded()) // Allow movement only if not on a wall or grounded
        {
            _body.linearVelocity = new Vector2(_horizontalInput * _movementSpeed, _body.linearVelocity.y);
        }
        else if (onWall() && !isGrounded() && _body.linearVelocity.y > 0) // Stuck on wall and jumping
        {
            // Prevent sticking by reducing upward velocity
            _body.linearVelocity = new Vector2(0, _body.linearVelocity.y);
        }

        _animator.SetBool("running", _horizontalInput != 0);

        // Adjustable jump
        if (Input.GetKeyUp(KeyCode.Space) && _body.linearVelocity.y > 0)
        {
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, _body.linearVelocity.y / 2);
        }

        // Adjust falling behavior
        if (_body.linearVelocity.y < 0 && !isGrounded()) // Falling
        {
            _animator.SetBool("falling", true);
            _body.gravityScale = _fallingSpeed; // Apply falling speed directly
        }
        else if (_body.linearVelocity.y > 0 && !isGrounded()) // Jumping up
        {
            _animator.SetTrigger("jump");
            _animator.SetBool("falling", false);
        }
        else // Grounded
        {
            _animator.SetBool("falling", false);
            _body.gravityScale = _initialGravityScale; // Ensure gravity is normal when grounded
        }

        /*
            JUMP LOGIC
        */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, _jumpForce);
        }
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, new Vector2(transform.localScale.x, 0), _rayLength, _groundLayer);
        return raycastHit.collider != null;
    }
    #endregion
}
