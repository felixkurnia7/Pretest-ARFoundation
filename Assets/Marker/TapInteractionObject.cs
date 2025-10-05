using UnityEngine;

public class TapInteractionObject : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private Camera _mainCamera;

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the user touched
        if (_inputActions.Home.Touch.triggered)
        {
            Vector2 touchPosition = _inputActions.Home.Touch.ReadValue<Vector2>();
            //ebug.Log("Tap position: " + touchPosition);
            if (touchPosition == Vector2.zero) return;

            Ray ray = _mainCamera.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the object has a Helicopter script
                Heicopter helicopter = hit.collider.GetComponentInParent<Heicopter>();
                if (helicopter != null)
                {
                    helicopter.OnTapped(); // show or hide trivia
                }
            }
        }
    }
}
