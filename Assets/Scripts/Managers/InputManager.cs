using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] PlayerInput playerInput;
    public bool isPaused;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        playerInput.actions.FindAction("Pause").performed += ctx => Pause();
    }

    public void Pause()
    {
        if (!isPaused)
        {
            UIManager.Instance.SettingsUI.rootVisualElement.Q<VisualElement>("SettingsParent").style.display = DisplayStyle.Flex;
        }
        else
        {
            UIManager.Instance.SettingsUI.rootVisualElement.Q<VisualElement>("SettingsParent").style.display = DisplayStyle.None;
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
        isPaused = !isPaused;
    }
}