using System.Collections;
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
    [SerializeField] private float _movementLagInSeconds = 0.1f;

    // References
    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _collider;

    // Variables
    private float _horizontalInput;
    private readonly float _rayLength = 0.01f;
    private float _initialGravityScale;
    private bool _isMovementDisabled = false;

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
        if (_isMovementDisabled) return;

        _horizontalInput = Input.GetAxis("Horizontal");

        /*
            MOVE CHARACTER LOGIC
        */

        // Move player and flip character
        FlipCharacter(_horizontalInput);
        if (!onWall() || isGrounded()) // Allow movement only if not on a wall or grounded
        {
            _body.linearVelocity = new Vector2(_horizontalInput * _movementSpeed, _body.linearVelocity.y);
            _animator.SetBool("grounded", true);
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
            _animator.SetBool("grounded", false);
            _body.gravityScale = _fallingSpeed;
        }
        else if (_body.linearVelocity.y > 0 && !isGrounded())
        {
            _animator.SetBool("falling", false);
            _animator.SetBool("grounded", false);
        }
        else // Grounded
        {
            _animator.SetBool("falling", false);
            _body.gravityScale = _initialGravityScale;
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

    /// <summary>
    /// Stops all player movement temporarily for a specified duration.
    /// </summary>
    /// <remarks>
    /// This method must be called using <c>StartCoroutine</c> to work correctly, 
    /// as it uses a coroutine to handle the delay.
    /// </remarks> 
    public IEnumerator StopMovement()
    {   
        if (!isGrounded()) yield return new WaitForSeconds(0);

        _isMovementDisabled = true;
        // Save current gravity scale to restore it later
        float originalGravityScale = _body.gravityScale;

        // Completely stop movement
        _body.linearVelocity = new Vector2(0, _body.linearVelocity.y);
        _horizontalInput = 0;

        // Reset animations to idle
        _animator.SetBool("running", false);
        _animator.SetBool("falling", false);
        _animator.SetBool("grounded", true);

        yield return new WaitForSeconds(_movementLagInSeconds);

        // Restore gravity and allow movement
        _body.gravityScale = originalGravityScale;
        _isMovementDisabled = false;
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
