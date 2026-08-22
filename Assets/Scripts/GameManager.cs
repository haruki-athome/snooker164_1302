using System;
using Unity.VisualScripting;
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

    public static GameManager instance;
	// Start is called once before the first execution of Update after the MonoBehaviour is created

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
        SetBall(BallColor.Red, 1);
        SetBall(BallColor.Green, 2);
        SetBall(BallColor.Yellow, 3);
        SetBall(BallColor.Brown, 4);
        SetBall(BallColor.Blue, 5);
        SetBall(BallColor.Black, 6);
        SetBall(BallColor.Pink, 7);

        if(Setting.fromSave)
        LoadGame();

        CameraBehindPoolball();
    }

    // Update is called once per frame
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
    private void SetBall(BallColor color, int i)
    {
        GameObject ball = Instantiate(ballPrefab,
                          ballPositions[i].transform.position,
                          Quaternion.identity);
        Ball b = ball.GetComponent<Ball>();
        b.SetColorandPoint(color);
    }
    private void ShootBall()
    {
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
        notiText.text = $"Ball Points: {score}\n Player Score: {playerScore}";
    }

    public void ShowString(string message)
    {
        notiText.text = message;
    }
    public void SaveGame()
    {
        if (cueball != null)
        {
            PlayerPrefs.SetFloat("CueballX", cueball.transform.position.x);
            PlayerPrefs.SetFloat("CueballY", cueball.transform.position.y);
            PlayerPrefs.SetFloat("CueballZ", cueball.transform.position.z);
            Debug.Log("Game Saved");
        }
        
	}
	public void LoadGame()
	{
		if (cueball != null)
		{
			float X = PlayerPrefs.GetFloat("CueballX");
			float Y = PlayerPrefs.GetFloat("CueballY");
			float Z = PlayerPrefs.GetFloat("CueballZ");
			cueball.transform.position = new Vector3(X, Y, Z);
			Debug.Log("Game Loaded");
		}

	}
}