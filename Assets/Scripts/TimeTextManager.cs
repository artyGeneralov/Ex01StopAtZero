using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTextManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI txt;
    int curTime;
    float lastTime;
    // Start is called before the first frame update
    void Start()
    {
        curTime = 0;
        lastTime = Time.time;
        displayTime();
        showClock(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (curTime != 0)
            tick();

    }

    void tick()
    {
        // every second
        if (Time.time - lastTime >= 1)
        {
            curTime--;
            lastTime = Time.time;
            displayTime();
        }
        // decrease time by 1
    }

    public void setCurTime(int time)
    {
        curTime = time;
        displayTime();
    }
    public int getCurrentTime()
    {
        return curTime;
    }

    void displayTime()
    {
        txt.text = "Time: " + curTime;
    }

    public void showClock(bool show)
    {
        if (show == false)
            txt.alpha = 0.1f;
        else
            txt.alpha = 1f;
    }
    
}
