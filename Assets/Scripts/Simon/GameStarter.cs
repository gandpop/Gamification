using UnityEngine;
using UnityEngine.UIElements;

public class GameStarter : MonoBehaviour
{
    public UIDocument uiDoc;
    public Button startButton;
    public GameObject stuff;

    void Awake()
    {
        uiDoc = GetComponent<UIDocument>();
        VisualElement root = uiDoc.rootVisualElement;
        startButton = root.Q<Button>("StartGame");
    }


    void OnEnable()
    {
        startButton.clicked += Vamos;
    }

    void Vamos()
    {
        stuff.SetActive(true);
        GameManager.Instance.StartGame();
        uiDoc.enabled = false;
    }
}
