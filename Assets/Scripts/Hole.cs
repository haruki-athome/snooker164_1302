using UnityEngine;

public class Hole : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Ball ball = other.GetComponent<Ball>();

		if (ball != null)
		{
			if (ball.Point == 0)
			{
				GameManager.instance.PlayerLost();
			}
			else
			{
				AudioManager.instance.PlayScorePoint();
				GameManager.instance.ShowScoreText(ball.Point);
				GameManager.instance.BallRemoved();
			}

			Destroy(ball.gameObject);
		}
	}
}