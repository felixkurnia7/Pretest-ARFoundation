using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Heicopter : MonoBehaviour
{
    [SerializeField] private Canvas triviaCanvas;
    [SerializeField] private float fadeDuration = 0.4f;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        if (triviaCanvas == null)
            triviaCanvas = GetComponentInChildren<Canvas>(true);

        triviaCanvas.worldCamera = Camera.main;

        // Add CanvasGroup for fading
        canvasGroup = triviaCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = triviaCanvas.gameObject.AddComponent<CanvasGroup>();

        triviaCanvas.gameObject.SetActive(false);
        canvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (triviaCanvas != null)
        {
            triviaCanvas.transform.LookAt(Camera.main.transform);
            triviaCanvas.transform.Rotate(0, 180, 0);
        }
    }

    public void OnTapped()
    {
        bool isActive = triviaCanvas.gameObject.activeSelf;

        if (!isActive)
        {
            triviaCanvas.gameObject.SetActive(true);
            LeanTween.alphaCanvas(canvasGroup, 1f, fadeDuration);
        }
        else
        {
            LeanTween.alphaCanvas(canvasGroup, 0f, fadeDuration)
                     .setOnComplete(() => triviaCanvas.gameObject.SetActive(false));
        }
    }
}
