using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	[SerializeField] private Slider ambientSlider;
	[SerializeField] private Slider sfxSlider;

	private void Awake()
	{
		// Diagnostic check: Are references connected?
		if (ambientSlider == null) Debug.LogError("[SettingsManager] Ambient Slider is NOT assigned in the Inspector!");
		if (sfxSlider == null) Debug.LogError("[SettingsManager] SFX Slider is NOT assigned in the Inspector!");
	}

	private void OnEnable()
	{
		SyncSettings();
	}

	public void SyncSettings()
	{
		float savedAmbient = PlayerPrefs.GetFloat("AmbientVolume", 1f);
		float savedSfx = PlayerPrefs.GetFloat("SfxVolume", 1f);

		Debug.Log($"[SettingsManager] Loaded from PlayerPrefs -> Ambient: {savedAmbient}, SFX: {savedSfx}");

		if (ambientSlider != null)
		{
			ambientSlider.SetValueWithoutNotify(savedAmbient);
		}

		if (sfxSlider != null)
		{
			sfxSlider.SetValueWithoutNotify(savedSfx);
		}

		if (AudioManager.instance != null)
		{
			AudioManager.instance.SetAmbientVolume(savedAmbient);
			AudioManager.instance.SetSfxVolume(savedSfx);
		}
	}

	public void OnAmbientVolumeChanged(float value)
	{
		Debug.Log($"[SettingsManager] OnAmbientVolumeChanged fired with value: {value}");
		if (AudioManager.instance != null)
		{
			AudioManager.instance.SetAmbientVolume(value);
		}
		PlayerPrefs.SetFloat("AmbientVolume", value);
		PlayerPrefs.Save();
	}

	public void OnSfxVolumeChanged(float value)
	{
		Debug.Log($"[SettingsManager] OnSfxVolumeChanged fired with value: {value}");
		if (AudioManager.instance != null)
		{
			AudioManager.instance.SetSfxVolume(value);
		}
		PlayerPrefs.SetFloat("SfxVolume", value);
		PlayerPrefs.Save();
	}
}