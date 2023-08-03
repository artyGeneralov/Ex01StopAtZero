using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    float motivation; // 1 to 10
    float baseSpeed = 1f;
    float friction = 0.9f;
    float maxFriction = 0.997f;
    float minFriction = 0.8f;
    bool canMove;
    float speed;
    enum State
    {
        TryingToMove,
        TryingToStop,
        Stopped
    }
    State curState;
    // Start is called before the first frame update
    void Start()
    {
        generateMotivation();
        canMove = false;
        speed = 0f;
        curState = State.Stopped;

    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;
        switch (curState)
        {
            case State.TryingToMove:
                moveSelf();
                break;
            case State.TryingToStop:
                break;
            case State.Stopped:
                break;
            default:
                break;
        }
    }
    public void setMoving(bool move)
    {
        if(move == false)
        {
            canMove = false;
            curState = State.Stopped;
        }
        else
        {
            canMove = true;
            curState = State.TryingToMove;
        }
    }
    void moveSelf()
    {
        // chance of 1 to 10 set by motivation:
        // 1 to 2 - move slower
        // 2 to 8 - dont change
        // 8 to 10 - go faster
    }

    void stopSelf()
    {

    }
    void generateMotivation()
    {
        motivation = Random.Range(1, 10);
    }
}
