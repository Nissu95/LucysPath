using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    float timer = 0;
    float time = 0;


    public void SetTime(float _time)
    {
        time = _time;
        Reset();
    }

    public void Reset()
    {
        timer = time;
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
        if (timer > 0)
            timer -= Time.deltaTime;
    }
}
