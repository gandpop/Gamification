using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Documents (set automatically)")]
    [SerializeField] UIDocument mainMenuUI;
    [SerializeField] UIDocument inGameUI;
    [SerializeField] UIDocument gameLostUI;
    [SerializeField] UIDocument gameWonUI;
    [SerializeField] UIDocument achievementUI;
    [SerializeField] UIDocument cheatSheetUI;

    // Public properties so they can be referred to elsewhere
    // while still having the varialbe private AND visible in Inspector
    public UIDocument MainMenuUI => mainMenuUI;
    public UIDocument InGameUI => inGameUI;
    public UIDocument GameLostUI => gameLostUI;
    public UIDocument GameWonUI => gameWonUI;
    public UIDocument AchievementUI => achievementUI;
    public UIDocument CheatSheetUI => cheatSheetUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        /*mainMenuUI = GetComponentInChildren<UIDocument>();
        inGameUI = GetComponentInChildren<UIDocument>();
        gameLostUI = GetComponentInChildren<UIDocument>();
        gameWonUI = GetComponentInChildren<UIDocument>();
        achievementUI = GetComponentInChildren<UIDocument>();
        cheatSheetUI = GetComponentInChildren<UIDocument>();

        if (mainMenuUI == null) Debug.LogWarning("UIManager: mainMenuUI missing");
        if (inGameUI == null) Debug.LogWarning("UIManager: inGameUI missing");
        if (gameLostUI == null) Debug.LogWarning("UIManager: gameLostUI missing");
        if (gameWonUI == null) Debug.LogWarning("UIManager: gameWonUI missing");*/
    }

    public void UpdateUI()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.MainMenu:
                mainMenuUI.enabled = true;
                inGameUI.enabled = false;
                gameLostUI.enabled = false;
                gameWonUI.enabled = false;
                achievementUI.enabled = true;
                cheatSheetUI.enabled = true;
                break;

            case GameState.InGame:
                mainMenuUI.enabled = false;
                inGameUI.enabled = true;
                gameLostUI.enabled = false;
                gameWonUI.enabled = false;
                achievementUI.enabled = true;
                cheatSheetUI.enabled = true;

                break;

            case GameState.GameOver:
                mainMenuUI.enabled = false;
                inGameUI.enabled = true;
                gameLostUI.enabled = true;
                gameWonUI.enabled = false;
                achievementUI.enabled = true;
                cheatSheetUI.enabled = true;

                break;

            case GameState.GameWon:
                mainMenuUI.enabled = false;
                inGameUI.enabled = true;
                gameLostUI.enabled = false;
                gameWonUI.enabled = true;
                achievementUI.enabled = true;
                cheatSheetUI.enabled = true;

                break;

        }
    }
}