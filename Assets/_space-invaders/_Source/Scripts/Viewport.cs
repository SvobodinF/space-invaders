using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Viewport : MonoBehaviour
{
    public static Viewport Instance;
    [SerializeField] private TMP_Text _livesLabel;
    [SerializeField] private TMP_Text _scoreLabel;
    [SerializeField] private CanvasGroup _gameOverScreen;
    [SerializeField] private CanvasGroup _winScreen;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continueButton;

    private InGameState _inGameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Initialize(InGameState inGameState)
    {
        _inGameState = inGameState;

        _restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        });

        _continueButton.onClick.AddListener(() =>
        {
            _inGameState.Continue();
            Time.timeScale = 1f;
        });

        ResetScreens();
    }

    public void GameOverScreenEnabled()
    {
        ChangeCanvasGroup(_gameOverScreen, true);
        ChangeCanvasGroup(_winScreen, false);
    }

    public void WinScreenEnabled(bool enabled)
    {
        ChangeCanvasGroup(_gameOverScreen, !enabled);
        ChangeCanvasGroup(_winScreen, enabled);
    }

    public void ResetScreens()
    {
        ChangeCanvasGroup(_gameOverScreen, false);
        ChangeCanvasGroup(_winScreen, false);
    }

    internal void ChangeScore(int value)
    {
        _scoreLabel.text = $"Score: {value}";
    }

    internal void ChangeHealth(int value)
    {
        _livesLabel.text = $"Lives: {value}";
    }

    private void ChangeCanvasGroup(CanvasGroup canvasGroup, bool enabled)
    {
        canvasGroup.interactable = enabled;
        canvasGroup.alpha = enabled == true ? 1f : 0f;
        canvasGroup.blocksRaycasts = enabled;
    }
}
