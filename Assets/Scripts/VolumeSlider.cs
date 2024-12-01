using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private string sliderName;
    [SerializeField] private string audioMixerParameter;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Icons")]
    [SerializeField] private Sprite highVolumeIcon;
    [SerializeField] private Sprite mediumVolumeIcon;
    [SerializeField] private Sprite lowVolumeIcon;
    [SerializeField] private Sprite noVolumeIcon;

    [Header("Intern Object")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI nameTextMesh;
    [SerializeField] private TextMeshProUGUI valueTextMesh;
    [SerializeField] private Image volumeIcon;

    private bool volumeActive = true;
    private float previousValue;

    // Start is called before the first frame update
    private void Start()
    {
        if (!audioMixer.GetFloat(audioMixerParameter, out float mixerValue))
        {
            Debug.LogError(audioMixerParameter + " is not a public parameter of the audioMixer " + audioMixer.ToString());
        }

        nameTextMesh.text = sliderName;

        // Get saved parameters in PlayerPrefs
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(audioMixerParameter, 1f));
        volumeActive = PlayerPrefs.GetInt(audioMixerParameter + "_active", 1) == 1;
        previousValue = slider.value;

        UpdateMixerValue();
    }

    public void SliderUpdate()
    {
        // Save into PlayerPrefs
        if (slider.value > previousValue)
        {
            volumeActive = true;
            PlayerPrefs.SetInt(audioMixerParameter + "_active", 1);
        }
        PlayerPrefs.SetFloat(audioMixerParameter, slider.value);
        PlayerPrefs.Save();
        previousValue = slider.value;

        UpdateMixerValue();
    }

    public void SwitchVolume()
    {
        volumeActive = !volumeActive;

        // Save into PlayerPrefs
        PlayerPrefs.SetInt(audioMixerParameter + "_active", volumeActive ? 1 : 0);
        PlayerPrefs.Save();

        UpdateMixerValue();
    }

    #region Update Objects
    private void UpdateMixerValue()
    {
        if (volumeActive)
        {
            audioMixer.SetFloat(audioMixerParameter, 20f * Mathf.Log10(slider.value));
        }
        else
        {
            audioMixer.SetFloat(audioMixerParameter, 20f * Mathf.Log10(slider.minValue));
        }

        valueTextMesh.text = ((int)(100 * slider.value)).ToString();

        UpdateSliderIcon();
    }

    private void UpdateSliderValue()
    {
        audioMixer.GetFloat(audioMixerParameter, out float mixerValue);

        mixerValue = Mathf.Pow(10, (mixerValue / 20f));

        slider.SetValueWithoutNotify(mixerValue);
        valueTextMesh.text = ((int)(100 * slider.value)).ToString();

        UpdateSliderIcon();
    }

    private void UpdateSliderIcon()
    {
        if (volumeActive && slider.value > slider.minValue)
        {
            if (slider.value < 0.33)
            {
                volumeIcon.sprite = lowVolumeIcon;
            }
            else if (slider.value > 0.66)
            {
                volumeIcon.sprite = highVolumeIcon;
            }
            else
            {
                volumeIcon.sprite = mediumVolumeIcon;
            }
        }
        else
        {
            volumeIcon.sprite = noVolumeIcon;
        }
    }
    #endregion Update Objects

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Selection.activeGameObject != this.gameObject) { return; }

        nameTextMesh.text = sliderName;

        bool test = audioMixer.GetFloat(audioMixerParameter, out float mixerValue);
        if (test)
        {
            Debug.Log("Parameter value : " + mixerValue);
        }
        UpdateSliderIcon();
    }
#endif
}
