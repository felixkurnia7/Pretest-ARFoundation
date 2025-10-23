using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ARScaleObject : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.1f;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 1f;
    private float _initialDistance;
    private Vector3 _initialScale;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (Touch.activeFingers.Count == 2)
        {
            var touch1 = Touch.activeFingers[0].currentTouch;
            var touch2 = Touch.activeFingers[1].currentTouch;

            float curDistance = Vector2.Distance(touch1.screenPosition, touch2.screenPosition);

            if (_initialDistance == 0)
            {
                _initialDistance = curDistance;
                _initialScale = ARPlaceObject.SpawnObject.transform.localScale;
            }
            else
            {
                if (ARPlaceObject.SpawnObject == null) return;

                float scaleFactor = (curDistance - _initialDistance) * scaleSpeed;
                ARPlaceObject.SpawnObject.transform.localScale = _initialScale * scaleFactor;
            }
        }
        else
        {
            _initialDistance = 0;
        }
    }
}
