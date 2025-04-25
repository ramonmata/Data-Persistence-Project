#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField inputField;
    
    [SerializeField]
    private TMP_Text bestScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameStateManager.Instance.BestPlayer != null)
        {
            string bestPlayerName = GameStateManager.Instance.BestPlayer.Name;
            int bestScore = GameStateManager.Instance.BestPlayer.BestScore;
            bestScoreText.text = "Best Score : " + bestPlayerName + " : " + bestScore;

            if (GameStateManager.Instance.Player != null)
            {
                inputField.text = GameStateManager.Instance.Player.Name;
            }
        }
    }

    public void StartGame()
    {
        if (inputField.text == "" || inputField.text == null)
        {
            SetInputFieldColor(Color.red);
            SetInputFieldPlaceholderColor(Color.white);
            inputField.ActivateInputField();
        }
        else
        {
            GameStateManager.Instance.FindOrCreatePlayer(inputField.text);
            SceneManager.LoadScene(1);
        }
    }

    public void RestoreInputFieldColor()
    {
        if (inputField.text.Length < 1)
        {
            SetInputFieldColor(Color.red);
            SetInputFieldPlaceholderColor(Color.white);
        }
        else
        {
            SetInputFieldColor(Color.white);
            SetInputFieldPlaceholderColor(Color.gray);
        }
    }

    public void SetInputFieldColor(Color color)
    {
        Image inputFieldBackground = inputField.GetComponent<Image>();
        if (inputFieldBackground != null)
        {
            inputFieldBackground.color = color;
        }
    }

    public void SetInputFieldPlaceholderColor(Color color)
    {
        
        if (inputField.placeholder != null)
        {
            TMP_Text placeholderText = inputField.placeholder as TMP_Text;
            if (placeholderText != null)
            {
                placeholderText.color = color;
            }
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
