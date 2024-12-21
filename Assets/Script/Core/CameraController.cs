using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Follow player
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _background;
    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _cameraSpeed;
    private float _offset;
    private void Update()
    {
        // Smoothly follow the target's position on both X and Y axes
        Vector3 targetPosition = new Vector3(
            _target.position.x + _offset,
            _target.position.y,
            transform.position.z
        );

        // Lerp to smooth the camera movement
        transform.position = targetPosition;

        // Update the offset for the X-axis based on the target's scale
        _offset = Mathf.Lerp(_offset, _cameraOffset * _target.localScale.x, Time.deltaTime * _cameraSpeed);
    }
}
