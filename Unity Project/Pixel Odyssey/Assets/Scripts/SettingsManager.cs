using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public CameraController cameraController;

    // Options
    [SerializeField] public float soundEffects = 1f;
    [SerializeField] public float music = 1f;
    [SerializeField] public float voice = 1f;
    [SerializeField] public float master = 1f;
    [SerializeField] public float sensitivity = 1f;

    private const float minVolumeDb = -80f; // Minimum dB value for the AudioMixer
    private const float maxVolumeDb = 0f; // Maximum dB value for the AudioMixer

    public AudioMixer audioMixer; // Reference to the AudioMixer
    public Slider soundEffectsSlider; // Reference to the Sound Effects slider
    public Slider musicSlider; // Reference to the Music slider
    public Slider voiceSlider; // Reference to the Voice slider
    public Slider masterSlider;  // Reference to the Master slider
    public Slider sensitivitySlider;  // Reference to the Sensitivity slider

    void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
        soundEffects = PlayerPrefs.GetFloat("SoundEffects", 1f); // Get the Sound Effects volume from PlayerPrefs
        music = PlayerPrefs.GetFloat("Music", 1f); // Get the Music volume from PlayerPrefs
        voice = PlayerPrefs.GetFloat("Voice", 1f); // Get the Voice volume from PlayerPrefs
        master = PlayerPrefs.GetFloat("Master", 1f);  // Get the Master volume from PlayerPrefs
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);  // Get the Sensitivity from PlayerPrefs
    }

    void Start()
    {

        SetVolume("Master", master); // Set the Master volume
        SetVolume("Music", music); // Set the Music volume
        SetVolume("SoundEffects", soundEffects); // Set the Sound Effects volume
        SetVolume("Voice", voice); // Set the Voice volume
        SetSensitivity(sensitivity); // Set the Sensitivity

        soundEffectsSlider.value = soundEffects; // Set the Sound Effects slider to the saved value
        musicSlider.value = music; // Set the Music slider to the saved value
        voiceSlider.value = voice; // Set the Voice slider to the saved value
        masterSlider.value = master; // Set the Master slider to the saved value
        sensitivitySlider.value = sensitivity; // Set the Sensitivity slider to the saved value

        soundEffectsSlider.onValueChanged.AddListener(SetSoundEffects); // Listen for Sound Effects slider changes
        musicSlider.onValueChanged.AddListener(SetMusic); // Listen for Music slider changes
        voiceSlider.onValueChanged.AddListener(SetVoice); // Listen for Voice slider changes
        masterSlider.onValueChanged.AddListener(SetMaster); // Listen for Master slider changes
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity); // Listen for Sensitivity slider changes
    }

    public void SetVolume(string parameterName, float value) // Set the volume in the AudioMixer
    {
        float dBValue; // The volume in decibels
        if (value <= 0.0001f) // Prevent negative infinity when converting to dB
        {
            dBValue = minVolumeDb; // Set to minimum dB value when slider is at minimum
        }
        else
        {
            dBValue = Mathf.Log10(value) * 20; // Convert linear volume to dB
        }

        bool result = audioMixer.SetFloat(parameterName, dBValue); // Set the volume in the AudioMixer
        
    }

    public void SetSensitivity(float value) // Set the camera sensitivity
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value); // Save the sensitivity to PlayerPrefs
        PlayerPrefs.Save(); // Ensure PlayerPrefs are saved
        cameraController.SetCameraSensitivity(sensitivity); // Update the camera sensitivity
    }

    public void SetSoundEffects(float value) // Set the Sound Effects volume
    {
        soundEffects = value;
        SetVolume("SoundEffects", value); // Ensure this matches the exposed parameter name
        PlayerPrefs.SetFloat("SoundEffects", soundEffects); // Save the Sound Effects volume to PlayerPrefs
        PlayerPrefs.Save(); // Save PlayerPrefs
    }

    public void SetMusic(float value) // Set the Music volume
    {
        music = value;
        SetVolume("Music", value); // Ensure this matches the exposed parameter name
        PlayerPrefs.SetFloat("Music", music); // Save the Music volume to PlayerPrefs
        PlayerPrefs.Save(); // Save PlayerPrefs
    }

    public void SetVoice(float value) // Set the Voice volume
    {
        voice = value;
        SetVolume("Voice", value); // Ensure this matches the exposed parameter name
        PlayerPrefs.SetFloat("Voice", voice); // Save the Voice volume to PlayerPrefs
        PlayerPrefs.Save(); // Save PlayerPrefs
    }

    public void SetMaster(float value) // Set the Master volume
    {
        master = value;
        SetVolume("Master", value); // Ensure this matches the exposed parameter name
        PlayerPrefs.SetFloat("Master", master); // Save the Master volume to PlayerPrefs
        PlayerPrefs.Save(); // Save PlayerPrefs
    }
}
