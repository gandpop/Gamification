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

        CurrentGameState = GameState.MainMenu;
    }

    void Start()
    {
        UIManager.Instance.UpdateUI();
    }

    public void StartGame()
    {
        PlayerHealth = 3;
        CurrentGameState = GameState.InGame;
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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.GameOver, transform, 0.2f);
        CurrentGameState = GameState.GameOver;
        UIManager.Instance.UpdateUI();
        Debug.Log("You ded");
    }

    public void GameWon()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.GameWon, transform, 1f);
        CurrentGameState = GameState.GameWon;
        UIManager.Instance.UpdateUI();
        Debug.Log("You win");
    }
}