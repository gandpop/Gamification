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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    

}