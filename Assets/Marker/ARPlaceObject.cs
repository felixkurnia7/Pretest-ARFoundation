using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using System.Collections;

public class ARPlaceObject : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;

    private InputSystem_Actions _inputAction;
    private bool isPlaced = false;
    public static GameObject spawnObject { get; private set; }


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
            if (spawnObject != null)
            {
                Destroy(spawnObject);
            }

            Vector3 hitPosition = rayHits[0].pose.position;
            Quaternion hitRotation = rayHits[0].pose.rotation;
            spawnObject = Instantiate(raycastManager.raycastPrefab, hitPosition, hitRotation);
            LeanTween.scale(spawnObject, Vector3.one * 0.1f, 1f).setEaseOutBack();
        }
    }

    public void RemoveObject()
    {
        isPlaced = false;
        if (spawnObject != null)
        {
            LeanTween.scale(spawnObject, Vector3.zero, 1f).setEaseInBack()
                .setOnComplete(() => Destroy(spawnObject));
        }

    }
}
