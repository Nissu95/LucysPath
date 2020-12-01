using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    float timer = 0;
    float time;
    bool active = true;

    public void SetTime(float _time)
    {
        time = _time;
        Reset();
    }

    public void Reset()
    {
        timer = time;
    }

    public void SetActive(bool _active)
    {
        active = _active;
    }

    public bool TimeUp()
    {
        if (timer <= 0)
            return true;
        else
            return false;
    }

    public void Update()
    {
        if (timer > 0 && active)
            timer -= Time.fixedDeltaTime;
    }
}
