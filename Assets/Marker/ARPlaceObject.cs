using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceObject : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;

    private InputSystem_Actions _inputAction;
    private bool isPlaced = false;
    private Quaternion _defaultRotation;
    private Vector3 _defaultScale = Vector3.one;
    public static GameObject SpawnObject { get; private set; }


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

    // Update is called once per frame
    void Update()
    {
        if (!raycastManager) return;

        if (_inputAction.Home.Touch.triggered && !isPlaced)
        {
            isPlaced = true;
            PlaceObject(_inputAction.Home.Touch.ReadValue<Vector2>());
        }
    }

    public void BackToStartScene()
    {
        SceneTransition.FadeAndLoad("StartScene", 1f);
    }

    void PlaceObject(Vector2 touchPosition)
    {
        var rayHits = new List<ARRaycastHit>();
        raycastManager.Raycast(touchPosition, rayHits, TrackableType.Planes);

        if (rayHits.Count > 0)
        {
            if (SpawnObject != null)
            {
                Destroy(SpawnObject);
            }

            Vector3 hitPosition = rayHits[0].pose.position;
            Quaternion hitRotation = rayHits[0].pose.rotation;
            SpawnObject = Instantiate(raycastManager.raycastPrefab, hitPosition, hitRotation);
            LeanTween.scale(SpawnObject, Vector3.one * 0.1f, 1f).setEaseOutBack().setOnComplete(() =>
            {
                _defaultRotation = SpawnObject.transform.rotation;
                _defaultScale = SpawnObject.transform.localScale;
            });
        }
    }

    public void PlayAnimation()
    {
        if (SpawnObject != null)
        {
            SpawnObject.GetComponent<HelicopterAnimation>()?.StartFlight();
        }
    }

    public void ResetObject()
    {
        if (SpawnObject != null)
        {
            //LeanTween.move(SpawnObject, Vector3.zero, 1f).setEaseInOutSine();
            LeanTween.rotate(SpawnObject, _defaultRotation.eulerAngles, 1f).setEaseOutQuad();
            LeanTween.scale(SpawnObject, _defaultScale, 1f).setEaseOutQuad();
        }
    }

    public void RemoveObject()
    {
        isPlaced = false;
        if (SpawnObject != null)
        {
            LeanTween.scale(SpawnObject, Vector3.zero, 1f).setEaseInBack()
                .setOnComplete(() => Destroy(SpawnObject));
        }

    }
}
