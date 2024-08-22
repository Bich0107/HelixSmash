using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Display()
    {
        Ball ball = FindObjectOfType<Ball>();
        text.text = "Your score: " + ball.BrickCounter;
    }
}
