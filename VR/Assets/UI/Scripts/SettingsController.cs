using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Furniture")]
    public FurniturePlacer placer;
    public Slider rotationSlider;

    [Header("Audio")]
    public Slider volumeSlider;

    [Header("UI Scale")]
    public Slider uiScaleSlider;
    public Transform floatingMenu;

    private const float defaultRotation = 90f;
    private const float defaultVolume = 80f;
    private const float defaultUIScale = 1f;

    private Vector3 originalMenuScale;

    void Start()
    {
        originalMenuScale = floatingMenu.localScale;

        // Rotation Speed
        rotationSlider.minValue = 30f;
        rotationSlider.maxValue = 180f;
        rotationSlider.wholeNumbers = false;
        rotationSlider.value = placer.rotationSpeed;
        rotationSlider.onValueChanged.AddListener(UpdateRotation);

        // Master Volume
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 100f;
        volumeSlider.wholeNumbers = true;
        volumeSlider.value = AudioListener.volume * 100f;
        volumeSlider.onValueChanged.AddListener(UpdateVolume);

        // UI Scale
        uiScaleSlider.minValue = 1f;
        uiScaleSlider.maxValue = 2f;
        uiScaleSlider.wholeNumbers = false;
        uiScaleSlider.value = defaultUIScale;
        uiScaleSlider.onValueChanged.AddListener(UpdateUIScale);

        UpdateUIScale(uiScaleSlider.value);
    }

    void UpdateRotation(float value)
    {
        placer.rotationSpeed = value;
    }

    void UpdateVolume(float value)
    {
        AudioListener.volume = value / 100f;
    }

    void UpdateUIScale(float value)
    {
        floatingMenu.localScale = originalMenuScale * value;
        Canvas.ForceUpdateCanvases();
    }

    public void ResetSettings()
    {
        rotationSlider.value = defaultRotation;
        volumeSlider.value = defaultVolume;
        uiScaleSlider.value = defaultUIScale;
    }
}