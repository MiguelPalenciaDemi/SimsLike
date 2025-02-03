using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float zoomSensitivity = 15f;
    
    [SerializeField] private float topLimit = 15f;
    [SerializeField] private float bottomLimit = -15f;
    [SerializeField] private float leftLimit = 15f;
    [SerializeField] private float rightLimit = -15f;
    
    [SerializeField] private float maxHeight = 15f;
    [SerializeField] private float lowHeight = 5f;
    
    
    private Vector2 _input;
    private float _height;
    
    public void MoveCameraInput(InputAction.CallbackContext ctx)
    {
        _input = ctx.ReadValue<Vector2>().normalized;
        Debug.Log(_input);
    }

    public void ZoomCameraInput(InputAction.CallbackContext ctx)
    {
        _height = ctx.ReadValue<float>();
        Zoom();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var targetPosition = new Vector3(
            Mathf.Clamp(transform.position.x - _input.x * moveSpeed * Time.deltaTime, leftLimit, rightLimit),
            transform.position.y,
            Mathf.Clamp(transform.position.z - _input.y * moveSpeed * Time.deltaTime, bottomLimit, topLimit)
        );
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed); 
    }

    private void Zoom()
    {
        var targetPosition = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y + _height * zoomSensitivity * Time.deltaTime, lowHeight, maxHeight),
            transform.position.z
        );
        transform.position = targetPosition; 

    }
}
