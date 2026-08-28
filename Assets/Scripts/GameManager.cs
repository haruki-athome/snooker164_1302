using System;
using System.Collections;
using System.Collections.Generic; // Added for List
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private int playerScore;
	public int PlayerScor { get { return playerScore; } set { playerScore = value; } }

	[SerializeField]
	private GameObject[] ballPositions;

	[SerializeField]
	private GameObject ballPrefab;

	[SerializeField]
	private GameObject cueball;

	[SerializeField]
	private float xInput = 0f;

	[SerializeField]
	private GameObject ballline;

	[SerializeField]
	private GameObject came;

	[SerializeField]
	private TMP_Text notiText;

	[SerializeField]
	private int ballsRemaining;
	[SerializeField]
	private GameObject restartPopup;
	[SerializeField]
	private TMP_Text restartText;

	public static GameManager instance;

	// Track active target balls on the table
	private List<Ball> activeBalls = new List<Ball>();

	private Coroutine notiCoroutine;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		if (notiText != null)
		{
			notiText.gameObject.SetActive(false);
		}

		if (Setting.fromSave)
		{
			LoadGame();
		}
		else
		{
			SpawnInitialBalls();
		}

		CameraBehindPoolball();
	}

	void Update()
	{
		RotateBall();

		if (Keyboard.current.spaceKey.wasPressedThisFrame)
			ShootBall();

		if (Keyboard.current.backspaceKey.wasPressedThisFrame)
			StopBall();

		if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
			xInput = -1f;
		else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
			xInput = 1f;
		else
			xInput = 0f;

		if (Keyboard.current.sKey.wasPressedThisFrame)
			SaveGame();
	}

	private void SpawnInitialBalls()
	{
		ballsRemaining = 7;
		playerScore = 0;
		ClearActiveBalls();

		SetBall(BallColor.Red, ballPositions[1].transform.position);
		SetBall(BallColor.Green, ballPositions[2].transform.position);
		SetBall(BallColor.Yellow, ballPositions[3].transform.position);
		SetBall(BallColor.Brown, ballPositions[4].transform.position);
		SetBall(BallColor.Blue, ballPositions[5].transform.position);
		SetBall(BallColor.Black, ballPositions[6].transform.position);
		SetBall(BallColor.Pink, ballPositions[7].transform.position);
	}

	private Ball SetBall(BallColor color, Vector3 spawnPosition)
	{
		GameObject ballObj = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
		Ball b = ballObj.GetComponent<Ball>();
		b.SetColorandPoint(color);
		activeBalls.Add(b);
		return b;
	}

	private void ClearActiveBalls()
	{
		foreach (Ball b in activeBalls)
		{
			if (b != null)
			{
				Destroy(b.gameObject);
			}
		}
		activeBalls.Clear();
	}

	private void ShootBall()
	{
		if (AudioManager.instance != null)
			AudioManager.instance.PlayPush();

		Rigidbody rd = cueball.GetComponent<Rigidbody>();
		rd.AddRelativeForce(Vector3.forward * 50f, ForceMode.Impulse);

		ballline.SetActive(false);

		came.transform.parent = null;
		came.transform.position = new Vector3(0f, 30f, -42f);
		came.transform.eulerAngles = new Vector3(45f, 0f, 0f);
	}

	private void RotateBall()
	{
		if (cueball != null)
		{
			cueball.transform.Rotate(new Vector3(0f, xInput, 0f));
		}
	}

	private void StopBall()
	{
		Rigidbody rd = cueball.GetComponent<Rigidbody>();
		rd.linearVelocity = Vector3.zero;
		rd.angularVelocity = Vector3.zero;
		cueball.transform.eulerAngles = new Vector3(0f, 0f, 0f);

		ballline.SetActive(true);
		CameraBehindPoolball();
	}

	private void CameraBehindPoolball()
	{
		came.transform.parent = cueball.transform;
		came.transform.position = cueball.transform.position + new Vector3(0f, 7f, -15f);
		came.transform.eulerAngles = new Vector3(30f, 0f, 0f);
	}

	public void ShowScoreText(int score)
	{
		playerScore += score;
		ShowNotification($"Ball Points: {score}\nPlayer Score: {playerScore}", 3f);
	}

	public void ShowString(string message)
	{
		ShowNotification(message, 5f);
	}

	public void ShowNotification(string message, float duration = 5f)
	{
		if (notiText == null) return;

		if (notiCoroutine != null)
		{
			StopCoroutine(notiCoroutine);
		}

		notiCoroutine = StartCoroutine(HideNotiAfterDelay(message, duration));
	}

	private IEnumerator HideNotiAfterDelay(string message, float duration)
	{
		notiText.text = message;
		notiText.gameObject.SetActive(true);

		yield return new WaitForSeconds(duration);

		notiText.gameObject.SetActive(false);
	}

	public void SaveGame()
	{
		// Remove potted/destroyed balls from list before saving
		activeBalls.RemoveAll(b => b == null);

		// 1. Save Cueball position
		if (cueball != null)
		{
			PlayerPrefs.SetFloat("CueballX", cueball.transform.position.x);
			PlayerPrefs.SetFloat("CueballY", cueball.transform.position.y);
			PlayerPrefs.SetFloat("CueballZ", cueball.transform.position.z);
		}

		// 2. Save Score & Remaining count
		PlayerPrefs.SetInt("PlayerScore", playerScore);
		PlayerPrefs.SetInt("BallsRemaining", ballsRemaining);

		// 3. Save Active Ball Count & positions/colors
		PlayerPrefs.SetInt("ActiveBallCount", activeBalls.Count);

		for (int i = 0; i < activeBalls.Count; i++)
		{
			Ball b = activeBalls[i];
			PlayerPrefs.SetInt($"Ball_{i}_Color", (int)b.color);
			PlayerPrefs.SetFloat($"Ball_{i}_X", b.transform.position.x);
			PlayerPrefs.SetFloat($"Ball_{i}_Y", b.transform.position.y);
			PlayerPrefs.SetFloat($"Ball_{i}_Z", b.transform.position.z);
		}

		PlayerPrefs.Save();
		Debug.Log("Game Saved");
		ShowNotification("Game Saved!", 5f);
	}

	public void LoadGame()
	{
		// Clear any default spawned balls
		ClearActiveBalls();

		// 1. Load Cueball position
		if (cueball != null && PlayerPrefs.HasKey("CueballX"))
		{
			float x = PlayerPrefs.GetFloat("CueballX");
			float y = PlayerPrefs.GetFloat("CueballY");
			float z = PlayerPrefs.GetFloat("CueballZ");
			cueball.transform.position = new Vector3(x, y, z);

			Rigidbody rd = cueball.GetComponent<Rigidbody>();
			if (rd != null)
			{
				rd.linearVelocity = Vector3.zero;
				rd.angularVelocity = Vector3.zero;
			}
		}

		// 2. Load Score & Balls Remaining
		playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
		ballsRemaining = PlayerPrefs.GetInt("BallsRemaining", 7);

		// 3. Re-spawn saved target balls
		int savedCount = PlayerPrefs.GetInt("ActiveBallCount", 0);
		for (int i = 0; i < savedCount; i++)
		{
			BallColor color = (BallColor)PlayerPrefs.GetInt($"Ball_{i}_Color");
			float x = PlayerPrefs.GetFloat($"Ball_{i}_X");
			float y = PlayerPrefs.GetFloat($"Ball_{i}_Y");
			float z = PlayerPrefs.GetFloat($"Ball_{i}_Z");

			SetBall(color, new Vector3(x, y, z));
		}

		Debug.Log("Game Loaded");
		ShowNotification("Game Loaded!", 3f);
	}

	public void BallRemoved()
	{
		ballsRemaining--;

		if (ballsRemaining <= 0)
		{
			if (AudioManager.instance != null)
				AudioManager.instance.PlayAllBallsGone();
			ShowRestartPopup($"ALL BALLS POTTED!\nFinal Score: {playerScore}");
		}
	}

	public void PlayerLost()
	{
		if (AudioManager.instance != null)
			AudioManager.instance.PlayLoseHole();
		ShowRestartPopup("YOU LOST!\nThe white ball went in.");
	}

	private void ShowRestartPopup(string message)
	{
		Time.timeScale = 0f;
		restartPopup.SetActive(true);
		restartText.text = message;
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}