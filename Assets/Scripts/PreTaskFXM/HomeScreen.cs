using CS.AudioToolkit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    private InputSystem_Actions _inputAction;

    [SerializeField]
    private GameObject title;
    [SerializeField]
    private GameObject startButton;

    private void OnEnable()
    {
        _inputAction.Enable();
        _inputAction.Home.Back.performed += ctx =>
        {
            SceneTransition.FadeOut(2f, QuitApplication);
        };
    }

    private void Awake()
    {
        _inputAction = new InputSystem_Actions();
    }

    private void Start()
    {
        //AudioController.Play("StartScene");
        LeanTween.move(title,
            new Vector3(title.transform.position.x, title.transform.position.y - 900, title.transform.position.z),
            1f).setEaseInOutSine();

        LeanTween.move(startButton,
            new Vector3(startButton.transform.position.x, startButton.transform.position.y + 1000, startButton.transform.position.z),
            1f).setEaseInOutSine();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    public void StartApp()
    {
        AudioController.Play("ButtonClick");
        SceneTransition.FadeAndLoad("Empty", 1f);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SceneTransition.FadeIn(2f, () => Application.Quit());
#endif
    }
}
