using UnityEngine;

public class PhasablePlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _player;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_player.linearVelocityY > 0.01f)
        {
            DisableCollision(true);
        }
        else if (_player.linearVelocityY < -0.01f)
        {
            DisableCollision(false);
        }
    }

    private void DisableCollision(bool toggle)
    {
        Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), _collider, toggle);
    }
}
