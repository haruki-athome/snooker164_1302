using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int playerScore;
    public int PlayerScor { get { return playerScore; } set { playerScore = value;}  }

    [SerializeField]
    private GameObject[] ballPositions;

    [SerializeField]
    private GameObject ballPrefab;

    public static GameManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBall(BallColor.White, 0);
		SetBall(BallColor.Red, 1);
        SetBall(BallColor.Green, 2);
        SetBall(BallColor.Yellow, 3);
		SetBall(BallColor.Brown, 4);
		SetBall(BallColor.Blue, 5);
		SetBall(BallColor.Black, 6);
		SetBall(BallColor.Pink, 7);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetBall(BallColor color,int i)
    {
    GameObject ball = Instantiate(ballPrefab,
                      ballPositions[i].transform.position, 
                      Quaternion.identity);
    Ball b = ball.GetComponent<Ball>();
    b.SetColorandPoint(color);
	}
}
