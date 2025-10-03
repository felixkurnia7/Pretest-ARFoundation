using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class SceneTransition
{
    private static bool isTransitioning = false;
    private static CanvasGroup canvasGroup;
    private static GameObject canvasObj;

    /// <summary>
    /// Ensures we have a transition canvas + canvas group
    /// </summary>
    private static void SetupCanvas()
    {
        if (canvasObj != null) return; // already exists

        // Create a Canvas
        canvasObj = new GameObject("SceneTransitionCanvas");
        var canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // on top
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create a background Image (black)
        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(canvasObj.transform, false);
        var img = imgObj.AddComponent<Image>();
        img.color = Color.black;

        RectTransform rect = img.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        // Add CanvasGroup
        canvasGroup = canvasObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; // start transparent
    }

    public static void FadeOut(float duration, System.Action onComplete = null)
    {
        SetupCanvas();
        isTransitioning = true;

        LeanTween.alphaCanvas(canvasGroup, 1f, duration).setOnComplete(() =>
        {
            isTransitioning = false;
            onComplete?.Invoke();
        });
    }

    public static void FadeIn(float duration, System.Action onComplete = null)
    {
        SetupCanvas();
        canvasGroup.alpha = 1f; // ensure fully black
        isTransitioning = true;

        LeanTween.alphaCanvas(canvasGroup, 0f, duration).setOnComplete(() =>
        {
            isTransitioning = false;
            onComplete?.Invoke();

            // Optionally clean up when fade is done
            Object.Destroy(canvasObj);
            canvasObj = null;
            canvasGroup = null;
        });
    }

    public static void FadeAndLoad(string sceneName, float duration = 0.8f)
    {
        if (isTransitioning) return;
        FadeOut(duration, () =>
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName);
        });

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            FadeIn(duration);
        }
    }
}
