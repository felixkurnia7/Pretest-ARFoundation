using CS.AudioToolkit;
using UnityEngine;
using UnityEngine.UI;

public class ARScene : MonoBehaviour
{
    [SerializeField] Button removeButton;
    private InputSystem_Actions _inputAction;

    private void OnEnable()
    {
        _inputAction.Enable();
        _inputAction.Home.Back.performed += ctx =>
        {
            AudioController.Play("Quit");
            SceneTransition.FadeOut(2f, QuitApplication);
        };
    }

    private void Awake()
    {
        _inputAction = new InputSystem_Actions();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioController.Play("ARScene");
    }

    public void PlayAnimation()
    {
        if (ARPlaceObject.SpawnObject != null)
        {
            AudioController.Play("Animation");
            ARPlaceObject.SpawnObject.GetComponent<HelicopterAnimation>()?.StartFlight();
        }
    }

    public void BackToStartScene()
    {
        AudioController.Play("ButtonBack");
        SceneTransition.FadeAndLoad("StartScene", 1f);
    }

    private void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
