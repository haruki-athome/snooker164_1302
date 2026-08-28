using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EscToMenu : MonoBehaviour
{
	void Update()
	{
		if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			GoToMenu();
		}
	}

	public void GoToMenu()
	{
		Time.timeScale = 1f; // unfreeze in case a win/lose popup paused the game
		PlayerPrefs.Save();
		SceneManager.LoadScene("MainMenu");
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		Setting.fromSave = false;
		SceneManager.LoadScene("Loading");
	}
}