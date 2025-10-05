using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class ARRotateObject : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycasterManager;
    [SerializeField] private float rotationSpeed = 0.2f;

    private InputSystem_Actions _inputAction;

    private bool _isDragging = false;
    private Vector2 _lastPosition;

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void Awake()
    {
        _inputAction = new InputSystem_Actions();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    private void Update()
    {
        bool isPressed = _inputAction.Home.TouchPress.ReadValue<float>() > 0.1f;
        Vector2 curPosition = _inputAction.Home.Touch.ReadValue<Vector2>();

        Debug.Log($"isPressed: {isPressed}, curPosition: {curPosition}");

        if (isPressed)
        {
            if (!_isDragging)
            {
                _isDragging = true;
                _lastPosition = curPosition;
            }
            else
            {
                Vector2 delta = curPosition - _lastPosition;
                ARPlaceObject.spawnObject.transform.Rotate(0, -delta.x * rotationSpeed, 0, Space.World);
                _lastPosition = curPosition;
            }
        }
        else
        {
            _isDragging = false;
        }
    }
}
