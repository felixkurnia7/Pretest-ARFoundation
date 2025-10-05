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

        if (_inputAction.Home.TapStartPosition.triggered && !isPlaced)
        {
            isPlaced = true;

            PlaceObject(_inputAction.Home.TapStartPosition.ReadValue<Vector2>());

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
            Vector3 hitPosition = rayHits[0].pose.position;
            Quaternion hitRotation = rayHits[0].pose.rotation;
            var spawnObject = Instantiate(raycastManager.raycastPrefab, hitPosition, hitRotation);
            LeanTween.scale(spawnObject, Vector3.one * 0.1f, 1f).setEaseOutBack();
        }

        StartCoroutine(SetIsPlaceToFalseDelayed());
    }

    IEnumerator SetIsPlaceToFalseDelayed()
    {
        yield return new WaitForSeconds(0.25f);
        isPlaced = false;
    }
}
