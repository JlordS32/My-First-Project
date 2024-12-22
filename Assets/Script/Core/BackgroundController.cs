using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // Reference to the camera
    [SerializeField] private float _parallaxEffect; // Parallax effect multiplier
    private float _startPosX, _length;

    private void Start()
    {
        _startPosX = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        // Use LateUpdate to minimize visual discrepancies caused by timing differences
        float temp = _camera.transform.position.x * (1 - _parallaxEffect);
        float distance = _camera.transform.position.x * _parallaxEffect;

        // Update the object's position based on parallax calculation
        transform.position = new Vector3(_startPosX + distance, transform.position.y, transform.position.z);

        // Infinite scrolling logic
        if (temp > _startPosX + _length)
        {
            _startPosX += _length * 2;
        }
        else if (temp < _startPosX - _length)
        {
            _startPosX -= _length * 2;
        }
    }
}
