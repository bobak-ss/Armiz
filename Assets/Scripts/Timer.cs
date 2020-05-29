using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Timer
{
    public float timerStartedAt;
    public float timerMax;

    public Timer(float _timerStartedAt, float _timerMax)
    {
        timerStartedAt = _timerStartedAt;
        timerMax = _timerMax;
    }

    public float GetTimerPassedPercent(float now)
    {
        return (now - timerStartedAt) / timerMax;
    }

    public bool isFinished(float now)
    {
        return ((now - timerStartedAt) > timerMax);
    }
}