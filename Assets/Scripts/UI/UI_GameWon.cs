using UnityEngine;
using UnityEngine.UIElements;

public class UI_GameWon : MonoBehaviour
{
    private UIDocument UIDocument;

    // Put all needed VisualElements fields here, and reference them in awake, similar to UI_InGame Script

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        else Debug.LogWarning("Game Won UI: Couldn't find UIDocument");

        VisualElement root = UIDocument.rootVisualElement;
    }
}