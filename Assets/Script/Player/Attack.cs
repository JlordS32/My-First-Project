using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;

    // References
    private Animator _animator;
    private PlayerMovement _playerMovement;

    // Variables
    private float attackTimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime; // Increment timer

        if (Input.GetMouseButtonDown(0) && attackTimer >= _attackCooldown)
        {
            StartCoroutine(_playerMovement.StopMovement());
            _animator.SetTrigger("attack");
            attackTimer = 0; // Reset cooldown
        }
    }
}
