using Unity.VisualScripting;
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
				GameManager.instance.ShowString($"son why u do EnsureThat to my ball");
				Time.timeScale = 0;
			}
			else
			{
				GameManager.instance.ShowScoreText(ball.Point);
			}

			Destroy(ball.gameObject);
		}
	}
		
}
