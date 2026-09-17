using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Settings : MonoBehaviour
{
    private UIDocument UIDocument;
    private VisualElement settingsParent;
    private Slider masterSlider;
    private Slider sfxSlider;
    private Slider musicSlider;
    private Button backButton;

    private List<Slider> sliders = new List<Slider>();

    void Awake()
    {
        if (UIDocument == null) UIDocument = GetComponent<UIDocument>();
        if (UIDocument == null) Debug.LogWarning("UI_Settings: Couldn't find UI Document");

        VisualElement root = UIDocument.rootVisualElement;
        settingsParent = root.Q<VisualElement>("SettingsParent");
        masterSlider = root.Q<Slider>("MasterSlider");
        sfxSlider = root.Q<Slider>("SoundFXSlider");
        musicSlider = root.Q<Slider>("MusicSlider");
        backButton = root.Q<Button>("BackButton");

        sliders = root.Query<Slider>().ToList();
    }

    void OnEnable()
    {
        masterSlider.RegisterValueChangedCallback(OnMasterSliderValueChanged);
        sfxSlider.RegisterValueChangedCallback(OnSFXSliderValueChanged);
        musicSlider.RegisterValueChangedCallback(OnMusicSliderValueChanged);

        backButton.RegisterCallback<MouseEnterEvent>(OnButtonHover);
        backButton.clicked += OnBackButtonClick;

        foreach (Slider slider in sliders)
        {
            slider.RegisterCallback<MouseEnterEvent>(OnButtonHover);
            slider.RegisterCallback<ClickEvent>(OnButtonClicked);
        }
    }

    void OnDisable()
    {
        masterSlider.UnregisterValueChangedCallback(OnMasterSliderValueChanged);
        sfxSlider.UnregisterValueChangedCallback(OnSFXSliderValueChanged);
        musicSlider.UnregisterValueChangedCallback(OnMusicSliderValueChanged);

        backButton.UnregisterCallback<MouseEnterEvent>(OnButtonHover);
        backButton.clicked -= OnBackButtonClick;

        foreach (Slider slider in sliders)
        {
            slider.UnregisterCallback<MouseEnterEvent>(OnButtonHover);
            slider.UnregisterCallback<ClickEvent>(OnButtonClicked);
        }
    }

    void OnMasterSliderValueChanged(ChangeEvent<float> evt)
    {
        AudioManager.Instance.SetMasterVolume(evt.newValue);
    }

    void OnSFXSliderValueChanged(ChangeEvent<float> evt)
    {
        AudioManager.Instance.SetSFXVolume(evt.newValue);
    }
       
    void OnMusicSliderValueChanged(ChangeEvent<float> evt)
    {
        AudioManager.Instance.SetMusicVolume(evt.newValue);
    }

    void OnButtonHover(MouseEnterEvent evt)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonHover, transform, 1f);
    }

    void OnButtonClicked(ClickEvent evt)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
    }

    void OnBackButtonClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.AudioData.ButtonClick, transform, 1f);
        settingsParent.style.display = DisplayStyle.None;
    }
}