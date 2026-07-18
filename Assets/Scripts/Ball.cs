using UnityEngine;
using UnityEngine.EventSystems;
public enum BallColor
{
    White,
    Red,
    Green,
    Yellow,
    Brown,
    Blue,
    Black,
    Pink
}

public class Ball : MonoBehaviour, IPointerClickHandler

{
    [SerializeField]
    private int point;

    [SerializeField]
    private BallColor color;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(point);
        GameManager.instance.PlayerScor += point;
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
