using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Greybox achievement drawer: hidden off-screen, pulled out by clicking the tab.
/// </summary>
public class AchievementPanelController : MonoBehaviour
{
    [System.Serializable]
    public struct AchievementData
    {
        public string title;
        public bool unlocked;
    }

    [SerializeField] private UIDocument uiDocument;

    [SerializeField]
    private List<AchievementData> achievements = new List<AchievementData>
    {
        new AchievementData { title = "Første Tjek", unlocked = false },
        new AchievementData { title = "Kildekritiker", unlocked = false },
        new AchievementData { title = "AI Detektiv", unlocked = false },
    };

    private VisualElement drawer;
    private Button tab;
    private ScrollView list;
    private bool isOpen;

    private void OnEnable()
    {
        if (uiDocument == null) uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;

        VisualElement root = uiDocument.rootVisualElement;
        drawer = root.Q<VisualElement>("achievement-drawer");
        tab = root.Q<Button>("achievement-tab");
        list = root.Q<ScrollView>("achievement-list");

        if (tab != null)
        {
            tab.clicked -= ToggleDrawer;
            tab.clicked += ToggleDrawer;
        }

        BuildAchievementList();
    }

    private void OnDisable()
    {
        if (tab != null) tab.clicked -= ToggleDrawer;
    }

    // Refreshes the drawer when a field is toggled in the Inspector during Play mode
    private void OnValidate()
    {
        if (Application.isPlaying) BuildAchievementList();
    }

    private void ToggleDrawer()
    {
        isOpen = !isOpen;
        drawer.EnableInClassList("drawer--open", isOpen);
        drawer.EnableInClassList("drawer--closed", !isOpen);
    }

    private void BuildAchievementList()
    {
        if (list == null) return;
        list.Clear();

        foreach (AchievementData achievement in achievements)
        {
            var row = new VisualElement();
            row.AddToClassList("achievement-row");
            row.EnableInClassList("achievement-row--locked", !achievement.unlocked);

            var icon = new VisualElement();
            icon.AddToClassList("achievement-icon");

            var label = new Label(achievement.title);
            label.AddToClassList("achievement-label");

            row.Add(icon);
            row.Add(label);
            list.Add(row);
        }
    }

    public void Unlock(string title)
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i].title == title)
            {
                AchievementData a = achievements[i];
                a.unlocked = true;
                achievements[i] = a;
            }
        }

        BuildAchievementList();
    }
}
