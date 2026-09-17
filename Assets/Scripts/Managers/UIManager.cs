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

    // Public properties so they can be referred to elsewhere
    // while still having the varialbe private AND visible in Inspector
    public UIDocument MainMenuUI => mainMenuUI;
    public UIDocument InGameUI => inGameUI;
    public UIDocument GameLostUI => gameLostUI;
    public UIDocument GameWonUI => gameWonUI;
    public UIDocument AchievementUI => achievementUI;
    public UIDocument CheatSheetUI => cheatSheetUI;

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
}