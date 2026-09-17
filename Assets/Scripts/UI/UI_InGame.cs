using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_InGame : MonoBehaviour
{
    // UI document and buttons
    private VisualElement root;
    private UIDocument UIDocument;
    private Button tjekButton;
    private DropdownField tendensDropdown;
    private DropdownField aegthedDropdown;
    private DropdownField afhaengighedDropdown;
    private DropdownField afsenderDropdown;
    private DropdownField vidensniveauDropdown;
    private DropdownField tidDropdown;

    public List<DropdownField> allDropDowns { get; private set; } = new List<DropdownField>();

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("UIScript: Couldn't find UIDocument");
        
        // Reference all UI elements
        root = UIDocument.rootVisualElement;
        tendensDropdown = root.Q<DropdownField>("Tendens");
        aegthedDropdown = root.Q<DropdownField>("Aegthed");
        afhaengighedDropdown = root.Q<DropdownField>("Afhaengighed");
        afsenderDropdown = root.Q<DropdownField>("Afsender");
        vidensniveauDropdown = root.Q<DropdownField>("Vidensniveau");
        tidDropdown = root.Q<DropdownField>("Tid");
        tjekButton = root.Q<Button>("Tjek");

        VisualElement dropdownParent = root.Q<VisualElement>("DropdownsParent");
        allDropDowns = dropdownParent.Query<DropdownField>().ToList();
    }
    
    void OnEnable()
    {
        tjekButton.clicked += OnCheckButtonClicked;
        tjekButton.RegisterCallback<MouseEnterEvent>(OnButtonHover);

        foreach (DropdownField dropdowns in allDropDowns)
        {
            dropdowns.RegisterCallback<PointerDownEvent>(OnFieldClicked);
            dropdowns.RegisterCallback<MouseEnterEvent>(OnButtonHover);
            dropdowns.RegisterValueChangedCallback(OnItemSelected);
        }
    }

    void OnDisable()
    {
        tjekButton.clicked -= OnCheckButtonClicked;
        tjekButton.UnregisterCallback<MouseEnterEvent>(OnButtonHover);

        foreach (DropdownField dropdowns in allDropDowns)
        {
            dropdowns.UnregisterCallback<PointerDownEvent>(OnFieldClicked);
            dropdowns.UnregisterCallback<MouseEnterEvent>(OnButtonHover);
            dropdowns.UnregisterValueChangedCallback(OnItemSelected);
        }
    }

    void OnCheckButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
        List<int> items = new List<int>()
        {
            tendensDropdown.index,
            aegthedDropdown.index,
            afhaengighedDropdown.index,
            afsenderDropdown.index,
            vidensniveauDropdown.index,
            tidDropdown.index,
        };

        List<int> answerValue = new List<int>();
        foreach (int index in items)
        {
            if (index == -1)
            {
                answerValue.Add(index);
            }
        }
        if (answerValue.Count != 0) AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ErrorSound, transform, 0.06f);
        else SourceManager.Instance.CheckAnswers(items); // Only check answers, if all fields have a value
    }

    void OnFieldClicked(PointerDownEvent evt)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
    }

    void OnItemSelected(ChangeEvent<string> evt)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
    }

    void OnButtonHover(MouseEnterEvent evt)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonHover, transform, 1f);
    }
}