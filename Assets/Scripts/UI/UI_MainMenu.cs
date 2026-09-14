using UnityEngine;
using UnityEngine.UIElements;

public class UI_MainMenu : MonoBehaviour
{
    private UIDocument UIDocument;

    // Put all needed VisualElements fields here, and reference them in awake, similar to UI_InGame Script

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("Main Menu UI script: Couldn't find UIDocument");

        VisualElement root = UIDocument.rootVisualElement;
    }
}