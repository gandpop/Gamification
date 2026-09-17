using UnityEngine;
using UnityEngine.UIElements;

public class UI_MainMenu : MonoBehaviour
{
    private UIDocument UIDocument;

    // UI elements
    private Label titel;
    private Button startButton;
    private Button indstillingerButton;
    private Button lukButton;

    [Header("Title Animation")]
    [SerializeField] private float animDistance = 8f;
    [SerializeField] private float animSpeed = 2.5f;

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("Kan ikke finde Main Menu UI Document");

        // Reference all UI elements
        VisualElement root = UIDocument.rootVisualElement;
        titel = root.Q<Label>("Titel");
        startButton = root.Q<Button>("StartButton");
        indstillingerButton = root.Q<Button>("IndstillingerButton");
        lukButton = root.Q<Button>("LukButton");
    }

    void Update()
    {
        // Title Animation

        if (titel != null)
        {
            float yOffset = Mathf.Sin(Time.unscaledTime * animSpeed) * animDistance;
            titel.style.translate = new StyleTranslate(new Translate(0, yOffset));
        }
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

        if (titel != null)
        {
            titel.style.translate = StyleKeyword.Null;
        }
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
        // Lukker spillet. Ikke super brugbart til Web-Build tho 
        Application.Quit();
    }
}