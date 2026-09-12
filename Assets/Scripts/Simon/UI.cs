using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [Header("References (serialized only for debugging purposes)")]
    [SerializeField] private UIDocument uiDocument;

    // UI dropdown buttons
    private Button tjekButton;
    private DropdownField tendensDropdown;
    private DropdownField aegthedDropdown;
    private DropdownField afhaengighedDropdown;
    private DropdownField afsenderDropdown;
    private DropdownField vidensniveauDropdown;
    private DropdownField tidDropdown;

    private List<DropdownField> answers = new List<DropdownField>();


    void Awake()
    {
        if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("UIScript: Couldn't find UIDocument");

        /*if (sourceManager == null) sourceManager = FindAnyObjectByType<SourceManager>();
        else Debug.LogWarning("UIScript: Couldn't find SourceManager");*/
        
        // Reference all UI elements
        VisualElement root = uiDocument.rootVisualElement;
        tendensDropdown = root.Q<DropdownField>("Tendens");
        aegthedDropdown = root.Q<DropdownField>("Aegthed");
        afhaengighedDropdown = root.Q<DropdownField>("Afhaengighed");
        afsenderDropdown = root.Q<DropdownField>("Afsender");
        vidensniveauDropdown = root.Q<DropdownField>("Vidensniveau");
        tidDropdown = root.Q<DropdownField>("Tid");
        tjekButton = root.Q<Button>("Tjek");
    }

    void OnEnable()
    {
        tjekButton.clicked += OnCheckButtonClicked;
    }

    void OnDisable()
    {
        tjekButton.clicked -= OnCheckButtonClicked;
    }

    void OnCheckButtonClicked()
    {
        List<DropdownField> wrongAnswers = new List<DropdownField>();

        if (tendensDropdown.index != (int)SourceManager.Instance.ActiveArticle.Tendens) wrongAnswers.Add(tendensDropdown);
        if (aegthedDropdown.index != (int)SourceManager.Instance.ActiveArticle.Aegthed) wrongAnswers.Add(aegthedDropdown);
        if (afhaengighedDropdown.index != (int)SourceManager.Instance.ActiveArticle.Afhaengighed) wrongAnswers.Add(afhaengighedDropdown);
        if (afsenderDropdown.index != (int)SourceManager.Instance.ActiveArticle.Afsender) wrongAnswers.Add(afsenderDropdown);
        if (vidensniveauDropdown.index != (int)SourceManager.Instance.ActiveArticle.Vidensniveau) wrongAnswers.Add(vidensniveauDropdown);
        if (tidDropdown.index != (int)SourceManager.Instance.ActiveArticle.Tid) wrongAnswers.Add(tidDropdown);
        
        if (wrongAnswers.Count == 0)
        {
            //SourceCalculator.Instance.CalculateTrustRating(SourceManager.Instance.ActiveArticle);
            Debug.Log("Yeah baby");
        }
        else
        {
            Debug.Log("Something is wrong");
        }
    }
}