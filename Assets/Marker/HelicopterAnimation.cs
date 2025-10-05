using UnityEngine;

public class HelicopterAnimation : MonoBehaviour
{
    private Vector3 startPos;
    private bool isFlying = false;
    private Vector3[] path;

    [Header("Flight Settings")]
    [SerializeField] private float takeoffHeight = 2f;
    [SerializeField] private float takeoffDuration = 1.5f;
    [SerializeField] private float flyRadius = 2f;
    [SerializeField] private float flyDuration = 4f;
    [SerializeField] private float landDuration = 1.5f;

    [Header("Rotors (Optional)")]
    [SerializeField] private Transform mainRotor;
    [SerializeField] private Transform tailRotor;
    [SerializeField] private float rotorSpeed = 500f;

    void Start()
    {
        // Save starting position when spawned
        startPos = ARPlaceObject.SpawnObject.transform.position;
        CreatePath();
    }

    void Update()
    {
        // Spin rotors if flying
        if (isFlying)
        {
            if (mainRotor != null)
                mainRotor.Rotate(Vector3.up, rotorSpeed * Time.deltaTime, Space.Self);

            if (tailRotor != null)
                tailRotor.Rotate(Vector3.forward, rotorSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void StartFlight()
    {
        if (isFlying) return;
        isFlying = true;

        Debug.Log("Taking off...");

        // Step 1: Takeoff
        LeanTween.moveY(gameObject, startPos.y + takeoffHeight, takeoffDuration)
            .setEaseOutQuad()
            .setOnComplete(() =>
            {
                StartFly();
            });
    }

    private void CreatePath()
    {
        // Step 2: Create a smooth circular flight path
        int pathPoints = 12;
        path = new Vector3[pathPoints];
        for (int i = 0; i < pathPoints; i++)
        {
            float angle = (i / (float)(pathPoints - 1)) * Mathf.PI * 2;
            path[i] = startPos + new Vector3(Mathf.Cos(angle) * flyRadius, takeoffHeight, Mathf.Sin(angle) * flyRadius);
        }
    }

    private void StartFly()
    {
        Debug.Log("Flying around...");
        LeanTween.move(gameObject, path[1], 1f).setEaseInOutSine().setOnComplete(FlyAround); // Ensure starting point is correct
    }

    private void FlyAround()
    {
        LeanTween.moveSpline(gameObject, path, flyDuration)
            .setEaseInOutSine()
            .setOnComplete(Land);
    }

    private void Land()
    {
        Debug.Log("Landing...");

        // Step 3: Land exactly on starting position
        LeanTween.move(gameObject, startPos, landDuration)
            .setEaseInQuad()
            .setOnComplete(() =>
            {
                isFlying = false;
                Debug.Log("Landed on original position!");
            });
    }
}
