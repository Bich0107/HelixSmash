using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] Slider slider;

    void Start()
    {
        ball = FindObjectOfType<Ball>();

        Reset();
    }

    void Update()
    {
        SetValue();
    }

    void SetValue()
    {
        if (!ball.IsInvincible)
        {
            slider.maxValue = ball.BrickAmount;
            slider.value = ball.ContinuousCounter;
        }
        else
        {
            slider.maxValue = ball.InvincibleTime;
            slider.value = ball.InvincibleTime - ball.InvincibleTimer;
        }
    }

    public void Reset()
    {
        slider.maxValue = ball.BrickAmount;
        slider.value = 0f;
    }
}
