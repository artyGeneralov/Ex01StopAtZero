using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{

    enum States
    {
        START,
        COUNTING_HIDDEN,
        COUNTING_FINAL,
        EVALUATE_MOVEMENT,
        EVALUATION
    }
    TimeTextManager timeManager;
    PlayerMovement playerMovement;
    States currentState;
    // Start is called before the first frame update
    void Start()
    {
        // sets random times
        
        currentState = States.START;
        playerMovement = Object.FindObjectOfType<PlayerMovement>();
        StartCoroutine(setTimeWhenReady());
        
    }

    private IEnumerator setTimeWhenReady()
    {
        while (timeManager == null)
        {
            timeManager = Object.FindObjectOfType<TimeTextManager>();
            yield return null;
        }
        setNewTime();
    }

    // Update is called once per frame
    void Update()
    {
        int curTime = timeManager.getCurrentTime();

        
        switch (currentState)
        {
            case States.START:
               //Empty for now
                break;
            case States.COUNTING_HIDDEN:
                playerMovement.setMove(true);
                timeManager.showClock(false);
                if (curTime <= 3)
                    currentState = States.COUNTING_FINAL;
                break;
            case States.COUNTING_FINAL:
                timeManager.showClock(true);
                if (curTime == 0)
                    currentState = States.EVALUATE_MOVEMENT;
                    // show clock
                    // sound ?
                    break;
            case States.EVALUATE_MOVEMENT:
                Debug.Log("EVALUATE_MOVEMENT STATE");
                
                if(playerMovement.isMoving())
                    playerMovement.killPlayer();
                Invoke("setNewTime", 3.0f);
                currentState = States.EVALUATION;
                playerMovement.setMove(false);
                break;
            case States.EVALUATION:
                //Empty for now
                break;
            default:
                break;
        }
    }

    void setNewTime()
    {
        Debug.Log("setNewTime()");
        int randTime = Random.Range(5, 9);
        timeManager.setCurTime(randTime);
        currentState = States.COUNTING_HIDDEN;
    }

    
}
