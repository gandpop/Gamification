using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{
    [Header("Referencer")]
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private KildeScript targetKilde;
    [SerializeField] private SourceCalculator sourceCalculator;

    // UI Elements
    private Button tjekButton;
    private DropdownField tendensDropdown;
    private DropdownField aegthedDropdown;
    private DropdownField afhaengighedDropdown;
    private DropdownField afsenderDropdown;
    private DropdownField vidensniveauDropdown;
    private DropdownField tidDropdown;

    public KildeScript TargetKilde
    {
        get => targetKilde;
        set => targetKilde = value;
    }

    private void Awake()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }
    }

    private void Start()
    {
        if (targetKilde == null)
        {
            targetKilde = Object.FindAnyObjectByType<KildeScript>();
        }
    }

    private void OnEnable()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        if (uiDocument != null)
        {
            SetupUI();
        }
    }

    private void OnDisable()
    {
        if (tjekButton != null)
        {
            tjekButton.clicked -= OnTjekClicked;
        }
    }

    private void SetupUI()
    {
        VisualElement root = uiDocument.rootVisualElement;
        if (root == null) return;

        // Query dropdowns by name from KildeUI.uxml
        tendensDropdown = root.Q<DropdownField>("Tendens");
        aegthedDropdown = root.Q<DropdownField>("Aegthed");
        afhaengighedDropdown = root.Q<DropdownField>("Afhaengighed");
        afsenderDropdown = root.Q<DropdownField>("Afsender");
        vidensniveauDropdown = root.Q<DropdownField>("Vidensniveau");
        tidDropdown = root.Q<DropdownField>("Tid");

        // Query button
        tjekButton = root.Q<Button>("Tjek");
        if (tjekButton != null)
        {
            tjekButton.clicked -= OnTjekClicked; // Ensure no duplicate listener
            tjekButton.clicked += OnTjekClicked;
        }
        else
        {
            Debug.LogWarning("UIScript: Knappen 'Tjek' blev ikke fundet i UI Documentet.");
        }
    }

    private void OnTjekClicked()
    {
        CheckAnswers();
    }

    public bool CheckAnswers()
    {
        if (targetKilde == null)
        {
            targetKilde = Object.FindAnyObjectByType<KildeScript>();
        }

        if (targetKilde == null)
        {
            Debug.LogWarning("UIScript: Der er ingen aktiv KildeScript på skærmen at tjekke imod.");
            return false;
        }

        List<string> wrongCategories = new List<string>();

        // Check each category by index matching enum values
        if (afsenderDropdown == null || afsenderDropdown.index != (int)targetKilde.Afsender)
            wrongCategories.Add("Afsender");

        if (aegthedDropdown == null || aegthedDropdown.index != (int)targetKilde.Aegthed)
            wrongCategories.Add("Ægthed");

        if (tendensDropdown == null || tendensDropdown.index != (int)targetKilde.Tendens)
            wrongCategories.Add("Tendens");

        if (tidDropdown == null || tidDropdown.index != (int)targetKilde.Tid)
            wrongCategories.Add("Tid");

        if (afhaengighedDropdown == null || afhaengighedDropdown.index != (int)targetKilde.Afhaengighed)
            wrongCategories.Add("Afhængighed");

        if (vidensniveauDropdown == null || vidensniveauDropdown.index != (int)targetKilde.Vidensniveau)
            wrongCategories.Add("Vidensniveau");

        if (wrongCategories.Count == 0)
        {
            Debug.Log("Korrekt! Alle svar matcher kilden.");

            // Calculate trust rating for the source
            sourceCalculator = Object.FindAnyObjectByType<SourceCalculator>();
            
            if (sourceCalculator != null)
            {
                sourceCalculator.CalculateTrustRating(targetKilde);
            }
            else
            {
                Debug.LogWarning("UIScript: No SourceCalculator.");
            }

            return true;
        }
        else
        {
            Debug.Log($"Forkert! Svarene matcher ikke kilden. (Forkerte felter: {string.Join(", ", wrongCategories)})");
            return false;
        }
    }
}

