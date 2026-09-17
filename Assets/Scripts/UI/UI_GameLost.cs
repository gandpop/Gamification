using UnityEngine;
using UnityEngine.UIElements;

public class UI_GameLost : MonoBehaviour
{
    private UIDocument UIDocument;

    // UI Elements
    private Label titel;
    private Button retry;
    private Button backToMain;

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("Kan ikke finde Game Lost UI Document");

        // Reference all UI elements
        VisualElement root = UIDocument.rootVisualElement;
        titel = root.Q<Label>("Titel");
        retry = root.Q<Button>("Retry");
        backToMain = root.Q<Button>("BackToMain");

    }

    void OnEnable()
    {
        retry.clicked += OnRetryClicked;
        backToMain.clicked += OnBackToMainClicked;
    }

    void OnDisable()
    {
        retry.clicked -= OnRetryClicked;
        backToMain.clicked -= OnBackToMainClicked;
    }

    void OnRetryClicked()
    {
        Debug.Log("Prøver spillet igen");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    void OnBackToMainClicked()
    {
        Debug.Log("Går til menuen");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CurrentGameState = GameState.MainMenu;
        }
    }
}