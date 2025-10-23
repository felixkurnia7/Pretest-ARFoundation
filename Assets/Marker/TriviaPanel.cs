using CS.AudioToolkit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TriviaData
{
    public Sprite image;
    [TextArea] public string text;
}

public class TriviaPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image triviaImage;
    [SerializeField] private TextMeshProUGUI triviaText;
    [SerializeField] private Button nextButton;

    [Header("Trivia Data")]
    [SerializeField] private TriviaData[] triviaList;

    private int currentIndex = 0;

    private void Start()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(NextTrivia);

        ShowTrivia(0);
    }

    private void ShowTrivia(int index)
    {
        if (triviaList.Length == 0) return;

        currentIndex = Mathf.Clamp(index, 0, triviaList.Length - 1);

        triviaImage.sprite = triviaList[currentIndex].image;
        triviaText.text = triviaList[currentIndex].text;
    }

    private void NextTrivia()
    {

        AudioController.Play("ButtonClick");
        int nextIndex = (currentIndex + 1) % triviaList.Length;
        ShowTrivia(nextIndex);
    }
}
