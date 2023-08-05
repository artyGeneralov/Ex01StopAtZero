using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTextManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI txt;
    int curTime;
    float lastTime;
    Color currentColor;
    [SerializeField] Color timerColor, goColor, startColor;

    void Start()
    {
        curTime = 0;
        lastTime = Time.time;
        displayStartMessage();
        setColor(startColor);
    }


    void Update()
    {
        if (curTime != 0)
            tick();

    }

    void setColor(Color c)
    {
        if (currentColor != c)
            currentColor = txt.color = c;
            
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
        setColor(timerColor);
    }

    void displayGoText()
    {
        setColor(goColor);
        txt.text = "GO!";
    }
    
    void displayStartMessage()
    {
        txt.text = "Get Ready";
    }

    public void showClock(bool show)
    {
        if (show == false)
        {
            displayGoText();
            txt.alpha = 0.8f;

        }
        else
        {
            displayTime();
            txt.alpha = 1f;
        }
    }
    
}
