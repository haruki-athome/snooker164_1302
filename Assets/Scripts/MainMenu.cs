using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void StartGame()
	{
		Setting.fromSave = false;
		SceneManager.LoadScene("Loading");
	}
	public void LoadSaveGame()
	{
		Setting.fromSave = true;
		SceneManager.LoadScene("Loading");
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}
