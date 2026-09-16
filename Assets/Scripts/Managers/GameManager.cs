using UnityEngine;

#region GameStates enum
    public enum GameState
    {
        MainMenu,
        InGame,
        GameOver,
        GameWon,
    }
#endregion

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState;

    [Tooltip("How many articles should the game spawn (default of 5)")]
    public int HowManyArticles = 5;

    [Header("Player Stats")]
    public int PlayerHealth = 3;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartGame()
    {
        CurrentGameState = GameState.InGame;
        PlayerHealth = 3;
        UIManager.Instance.UpdateUI();
        SourceManager.Instance.PickArticles(HowManyArticles);
        SourceManager.Instance.ShowNextArticle();
        Debug.Log("Game started");
    }
    
    public void RemoveHealthPoint(int healthToRemove)
    {
        PlayerHealth -= healthToRemove;
    }

    public void GameLost()
    {
        CurrentGameState = GameState.GameOver;
        Debug.Log("You ded");
    }

    public void GameWon()
    {
        CurrentGameState = GameState.GameWon;
        Debug.Log("You win");
    }
}