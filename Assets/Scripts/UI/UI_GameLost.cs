using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class UI_GameLost : MonoBehaviour
{
    private UIDocument UIDocument;

    // UI Elements
    private VisualElement gameLostParent;
    private Label titel;
    private Button retry;
    private Button backToMain;
    private List<Button> allButtons = new List<Button>();

    [SerializeField] private float slideDuration = 0.8f;
    private bool wasShown = false;

    void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
        if (UIDocument == null)
        {
            Debug.LogWarning("Kan ikke finde Game Lost UI Document");
            return;
        }

        VisualElement root = UIDocument.rootVisualElement;
        gameLostParent = root.Q<VisualElement>("GameLostParent");
        titel = root.Q<Label>("Titel");
        retry = root.Q<Button>("Retry");
        backToMain = root.Q<Button>("BackToMain");

        if (gameLostParent != null)
            gameLostParent.style.translate = new StyleTranslate(new Translate(0, -Screen.height));

        allButtons = root.Query<Button>().ToList();
    }

    void Update()
    {
        if (gameLostParent == null) return;

        bool isShown = gameLostParent.style.display == DisplayStyle.Flex;
        if (isShown && !wasShown)
        {
            StopAllCoroutines();
            StartCoroutine(SlideIn());
        }
        else if (!isShown && wasShown)
        {
            gameLostParent.style.translate = new StyleTranslate(new Translate(0, -Screen.height));
        }

        wasShown = isShown;
    }

    private IEnumerator SlideIn()
    {
        float elapsed = 0f;
        float screenH = (UIDocument != null && UIDocument.rootVisualElement.layout.height > 0)
            ? UIDocument.rootVisualElement.layout.height
            : Screen.height;
        float startY = -screenH;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            float y = Mathf.Lerp(startY, 0, EaseOutBounce(t));

            gameLostParent.style.translate = new StyleTranslate(new Translate(0, y));
            yield return null;
        }

        gameLostParent.style.translate = new StyleTranslate(new Translate(0, 0));
    }

    private float EaseOutBounce(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1f / d1)
            return n1 * t * t;
        if (t < 2f / d1)
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        if (t < 2.5f / d1)
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    void OnEnable()
    {
        if (retry != null) retry.clicked += OnRetryClicked;
        if (backToMain != null) backToMain.clicked += OnBackToMainClicked;

        foreach (Button buttons in allButtons)
        {
            buttons.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        }
    }

    void OnDisable()
    {
        if (retry != null) retry.clicked -= OnRetryClicked;
        if (backToMain != null) backToMain.clicked -= OnBackToMainClicked;

        foreach (Button buttons in allButtons)
        {
            buttons.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        }
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
            SourceManager.Instance.ClearArticle();
            GameManager.Instance.CurrentGameState = GameState.MainMenu;
            UIManager.Instance.UpdateUI();
        }
    }

    void OnButtonHover(MouseEnterEvent evt)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonHover, transform, 1f);
    }
}