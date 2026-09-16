using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_InGame : MonoBehaviour
{
    // UI document and buttons
    private UIDocument UIDocument;
    private Button tjekButton;
    private DropdownField tendensDropdown;
    private DropdownField aegthedDropdown;
    private DropdownField afhaengighedDropdown;
    private DropdownField afsenderDropdown;
    private DropdownField vidensniveauDropdown;
    private DropdownField tidDropdown;


    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("UIScript: Couldn't find UIDocument");
        
        // Reference all UI elements
        VisualElement root = UIDocument.rootVisualElement;
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
        List<int> answers = new List<int>()
        {
            tendensDropdown.index,
            aegthedDropdown.index,
            afhaengighedDropdown.index,
            afsenderDropdown.index,
            vidensniveauDropdown.index,
            tidDropdown.index,
        };
        SourceManager.Instance.CheckAnswers(answers);
    }
}