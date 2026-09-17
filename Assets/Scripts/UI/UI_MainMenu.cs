using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class UI_MainMenu : MonoBehaviour
{
    private UIDocument UIDocument;

    // UI elements
    private Label titel;
    private Button startButton;
    private Button indstillingerButton;
    private Button lukButton;

    private List<Button> allButtons = new List<Button>();

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

        allButtons = root.Query<Button>().ToList();
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

        foreach (Button buttons in allButtons)
        {
            buttons.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        }       
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
        foreach (Button buttons in allButtons)
        {
            buttons.UnregisterCallback<MouseEnterEvent>(OnButtonHover);
        }
    }

    void OnStartButtonClicked()
    {
        Debug.Log("Starter Spil");
        if (GameManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
            GameManager.Instance.StartGame();
        }
    }

    void OnIndstillingerButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
        UIManager.Instance.SettingsUI.rootVisualElement.Q<VisualElement>("SettingsParent").style.display = DisplayStyle.Flex; // Enable settings ui
        InputManager.Instance.isPaused = true;
    }

    void OnLukButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
        Application.Quit();
    }

    void OnButtonHover(MouseEnterEvent evt)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonHover, transform, 1f);
    }
}