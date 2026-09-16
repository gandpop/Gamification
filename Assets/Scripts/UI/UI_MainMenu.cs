using UnityEngine;
using UnityEngine.UIElements;

public class UI_MainMenu : MonoBehaviour
{
    private UIDocument UIDocument;

    // UI elements
    private VisualElement titleContainer;
    private Label titelLabel;
    private VisualElement buttonContainer;
    private Button startButton;
    private Button indstillingerButton;
    private Button lukButton;

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("Kan ikke finde UI Document");

        // Reference all UI elements
        VisualElement root = UIDocument.rootVisualElement;
        titleContainer = root.Q<VisualElement>("TitleContainer");
        titelLabel = root.Q<Label>("Titel");
        buttonContainer = root.Q<VisualElement>("ButtonContainer");
        startButton = root.Q<Button>("StartButton");
        indstillingerButton = root.Q<Button>("IndstillingerButton");
        lukButton = root.Q<Button>("LukButton");
    }

    void OnEnable()
    {
        startButton.clicked += OnStartButtonClicked;
        indstillingerButton.clicked += OnIndstillingerButtonClicked;
        lukButton.clicked += OnLukButtonClicked;
    }

    void OnDisable()
    {
        startButton.clicked -= OnStartButtonClicked;
        indstillingerButton.clicked -= OnIndstillingerButtonClicked;
        lukButton.clicked -= OnLukButtonClicked;
    }

    void OnStartButtonClicked()
    {
        Debug.Log("Starter Spil");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    void OnIndstillingerButtonClicked()
    {
        Debug.Log("Indstillinger clicked");
    }

    void OnLukButtonClicked()
    {
        // Lukker spillet. Ikke så brugbart til Web-Build tho 
        Application.Quit();
    }
}