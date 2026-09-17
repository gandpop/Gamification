using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Documents (set manually)")]
    [SerializeField] UIDocument mainMenuUI;
    [SerializeField] UIDocument inGameUI;
    [SerializeField] UIDocument gameLostUI;
    [SerializeField] UIDocument gameWonUI;
    [SerializeField] UIDocument achievementUI;
    [SerializeField] UIDocument cheatSheetUI;
    [SerializeField] UIDocument scoreUI;

    [Header("Loading animation (Pspritesheet frames, set manually)")]
    [SerializeField] Sprite[] loadingFrames;
    [SerializeField] float loadingFrameInterval = 0.05f;
    [SerializeField] float loadingDuration = 1f;
    [SerializeField] Sprite lockSprite;

    // Public properties so they can be referred to elsewhere
    // while still having the varialbe private AND visible in Inspector
    public UIDocument MainMenuUI => mainMenuUI;
    public UIDocument InGameUI => inGameUI;
    public UIDocument GameLostUI => gameLostUI;
    public UIDocument GameWonUI => gameWonUI;
    public UIDocument AchievementUI => achievementUI;
    public UIDocument CheatSheetUI => cheatSheetUI;
    public UIDocument ScoreUI => scoreUI;

    [Header("UI Scripts (set automatically)")]
    [SerializeField] UI_InGame inGameUIScript;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        inGameUIScript = GetComponentInChildren<UI_InGame>();
    }

    public void UpdateUI()
    {
        var mainMenuParent = mainMenuUI.rootVisualElement.Q<VisualElement>("MainMenuParent");
        var inGameParent = inGameUI.rootVisualElement.Q<VisualElement>("InGameParent");
        var gameLostParent = gameLostUI.rootVisualElement.Q<VisualElement>("GameLostParent");
        var gameWonParent = gameWonUI.rootVisualElement.Q<VisualElement>("GameWonParent");
        var achievementParent = achievementUI.rootVisualElement.Q<VisualElement>("achievement-drawer");
        var cheatSheetParent = cheatSheetUI.rootVisualElement.Q<VisualElement>("CheatSheetParent");
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.MainMenu:
                mainMenuParent.style.display = DisplayStyle.Flex;
                inGameParent.style.display = DisplayStyle.None;
                gameLostParent.style.display = DisplayStyle.None;
                gameWonParent.style.display = DisplayStyle.None;
                achievementParent.style.display = DisplayStyle.Flex;
                cheatSheetParent.style.display = DisplayStyle.Flex;
                break;

            case GameState.InGame:
                mainMenuParent.style.display = DisplayStyle.None;
                inGameParent.style.display = DisplayStyle.Flex;
                gameLostParent.style.display = DisplayStyle.None;
                gameWonParent.style.display = DisplayStyle.None;
                achievementParent.style.display = DisplayStyle.Flex;
                cheatSheetParent.style.display = DisplayStyle.Flex;
                break;

            case GameState.GameOver:
                mainMenuParent.style.display = DisplayStyle.None;
                inGameParent.style.display = DisplayStyle.Flex;
                gameLostParent.style.display = DisplayStyle.Flex;
                gameWonParent.style.display = DisplayStyle.None;
                achievementParent.style.display = DisplayStyle.Flex;
                cheatSheetParent.style.display = DisplayStyle.Flex;
                break;

            case GameState.GameWon:
                mainMenuParent.style.display = DisplayStyle.Flex;
                inGameParent.style.display = DisplayStyle.None;
                gameLostParent.style.display = DisplayStyle.None;
                gameWonParent.style.display = DisplayStyle.Flex;
                achievementParent.style.display = DisplayStyle.Flex;
                cheatSheetParent.style.display = DisplayStyle.Flex;
                break;
        }
    }
    
    public void ClearAnswers()
    {
        List<DropdownField> answers = inGameUIScript.allDropDowns;

        foreach (DropdownField field in answers)
        {
            field.index = -1;
        }
    }

    /// <summary>
    /// Shows a brief "loading" beat, then reveals the trust rating, then invokes onComplete.
    /// </summary>
    public void ShowArticleScore(float trustRating, System.Action onComplete)
    {
        StartCoroutine(ScoreRevealRoutine(trustRating, onComplete));
    }

    IEnumerator ScoreRevealRoutine(float trustRating, System.Action onComplete)
    {
        VisualElement root = scoreUI.rootVisualElement;
        VisualElement scoreParent = root.Q<VisualElement>("ScoreParent");
        VisualElement loadingIcon = root.Q<VisualElement>("LoadingIcon");
        VisualElement lockContainer = root.Q<VisualElement>("LockContainer");
        VisualElement lockIcon = root.Q<VisualElement>("LockIcon");
        Label scoreLabel = root.Q<Label>("ScoreLabel");

        scoreParent.style.display = DisplayStyle.Flex;
        loadingIcon.style.display = DisplayStyle.Flex;
        lockContainer.style.display = DisplayStyle.None;

        // Cycle through the pspritesheet frames for loadingDuration seconds
        float elapsed = 0f;
        int frameIndex = 0;
        while (elapsed < loadingDuration)
        {
            loadingIcon.style.backgroundImage = new StyleBackground(loadingFrames[frameIndex % loadingFrames.Length]);
            frameIndex++;
            yield return new WaitForSeconds(loadingFrameInterval);
            elapsed += loadingFrameInterval;
        }

        loadingIcon.style.display = DisplayStyle.None;
        lockIcon.style.backgroundImage = new StyleBackground(lockSprite);
        scoreLabel.text = $"{trustRating:F0} / 100";
        lockContainer.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(2f);

        scoreParent.style.display = DisplayStyle.None;
        onComplete?.Invoke();
    }
}