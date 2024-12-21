using System.Collections;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _parallaxEffect;
    private float _startPosX, _length;

    private void Start()
    {
        _startPosX = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        // Calculate distance relative to cam pos and parallax effect
        float distance = _camera.transform.position.x * _parallaxEffect;
        float movement = _camera.transform.position.x * (1 - _parallaxEffect);

        // Update new distance
        transform.position = new Vector3(_startPosX + distance, transform.position.y, transform.position.z);

        // Infinite scrolling
        if (movement > _startPosX + _length)
        {
            _startPosX += _length * 2;
        }
        else if (movement < _startPosX - _length)
        {
            _startPosX -= _length * 2;
        }
    }
}
