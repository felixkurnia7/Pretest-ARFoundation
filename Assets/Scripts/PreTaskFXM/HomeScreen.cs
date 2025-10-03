using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public InputSystem_Actions _inputAction;

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

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    public void StartApp()
    {
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
