using UnityEngine;
using UnityEngine.UIElements;

public class UI_CheatSheet : MonoBehaviour
{
    private UIDocument UIDocument;
    private Button cheatSheetButton;
    bool menuOpen;

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("Can't find cheat sheet ui document");

        VisualElement root = UIDocument.rootVisualElement;
        cheatSheetButton = root.Q<Button>("CheatSheetButton");
    }

    void OnEnable()
    {
        cheatSheetButton.clicked += CheatSheetButtonClicked;
    }

    void OnDisable()
    {
        cheatSheetButton.clicked -= CheatSheetButtonClicked;
    }

    void CheatSheetButtonClicked()
    {
        Debug.Log("Achievement panel opened/close");
    }
}
