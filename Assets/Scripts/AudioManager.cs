using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	[SerializeField] private AudioSource ambientSource;
	[SerializeField] private AudioClip ambientClip;

	[SerializeField] private AudioSource sfxSource;
	[SerializeField] private AudioClip pushClip;
	[SerializeField] private AudioClip ballHitClip;
	[SerializeField] private AudioClip scorePointClip;
	[SerializeField] private AudioClip loseHoleClip;
	[SerializeField] private AudioClip allBallsGoneClip;
	[SerializeField] private AudioClip clickClip;

	public void PlayPush() => sfxSource.PlayOneShot(pushClip);
	public void PlayBallHit() => sfxSource.PlayOneShot(ballHitClip);
	public void PlayScorePoint() => sfxSource.PlayOneShot(scorePointClip);
	public void PlayLoseHole() => sfxSource.PlayOneShot(loseHoleClip);
	public void PlayAllBallsGone() => sfxSource.PlayOneShot(allBallsGoneClip);


	void Awake()
	{
		AudioListener.volume = 1f;

		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);

		ambientSource.clip = ambientClip;
		ambientSource.loop = true;

		float savedAmbient = PlayerPrefs.GetFloat("AmbientVolume", 1f);
		float savedSfx = PlayerPrefs.GetFloat("SfxVolume", 1f);

		ambientSource.volume = savedAmbient;
		sfxSource.volume = savedSfx;

		ambientSource.Play();
	}

	public void SetAmbientVolume(float value)
	{
		if (ambientSource != null)
			ambientSource.volume = value;
	}

	public void SetSfxVolume(float value)
	{
		if (sfxSource != null)
			sfxSource.volume = value;
	}

	public void PlayClick()
	{
		if (clickClip != null && sfxSource != null)
		{
			sfxSource.PlayOneShot(clickClip);
		}
	}
}