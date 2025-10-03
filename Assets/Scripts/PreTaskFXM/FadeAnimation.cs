using UnityEngine;

public class FadeAnimation : MonoBehaviour
{
    public static FadeAnimation Instance;

    [SerializeField]
    private CanvasGroup CanvasGroup;
    [SerializeField]
    private float FadeDuration = 2.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeIn()
    {
        LeanTween.alphaCanvas(CanvasGroup, 1, FadeDuration).setEase(LeanTweenType.easeInOutSine);
    }

    public void FadeOut()
    {
        LeanTween.alphaCanvas(CanvasGroup, 0, FadeDuration).setEase(LeanTweenType.easeInOutSine);
    }
}
