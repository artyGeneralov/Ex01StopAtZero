using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    int motivation; // 1 to 10
    [SerializeField] float baseSpeed = 0.3f;
    [SerializeField]float speedDecrease = 0.008f;
    bool canMove;
    float speed;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float speedEpsilon = 0.000015f;
    float timer = 0f;
    [SerializeField] float delay = 0.5f;

    Color movingColor = Color.green;
    Color stoppedColor = Color.cyan;
    Color deadColor = Color.red;
    Color victoryColor = Color.magenta;
    SpriteRenderer renderer;
    TimeTextManager timeManager;
    bool isAlive;
    enum State
    {
        TryingToMove,
        TryingToStop,
        Stopped,
        Dead,
        Winner
    }
    State curState;

    // Start is called before the first frame update
    void Start()
    {
        generateMotivation();
        renderer = GetComponent<SpriteRenderer>();
        canMove = false;
        speed = baseSpeed;
        curState = State.Stopped;
        isAlive = true;
        timeManager = Object.FindObjectOfType<TimeTextManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switch (curState)
        {
            case State.TryingToMove:
                if (!isAlive)
                    Destroy(gameObject);
                if (timer >= delay)
                {
                    calculateMovement();
                    timer = 0f;
                }
                moveSelf();
                break;
            case State.TryingToStop:
                stopSelf();
                moveSelf();
                break;
            case State.Stopped:

                break;
            case State.Winner:
                stopSelf();
                break;
            default:
                break;
        }
        setColors();
 
    }

    public bool isDead()
    {
        return !isAlive;
    }
    public void setMoving(bool move)
    {
        if (move == false)
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
            transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    void calculateMovement()
    {
        speed = baseSpeed;
        int rand = Random.Range(1, 11);
        int slowBound = Mathf.Clamp(10 - motivation, 1, 10); // motivation 10 - slow bound = 1 // motivation 1 - slow bound - 9
        int fastBound = Mathf.Clamp(1 + motivation, 1, 10);
        if (rand <= slowBound)
            speed -= speedEpsilon;
        else if (rand >= fastBound)
            speed += speedEpsilon;

        speed = Mathf.Clamp(speed, 0, maxSpeed);

        // decision to start stopping?
        if (timeManager.getCurrentTime() <= 2)
            if (rand >= 5)
                curState = State.TryingToStop;

    }

    public bool isMoving()
    {
        if (speed != 0f)
            return true;
        else
            return false;
    }

    void stopSelf()
    {
        if (speed <= 0.005f)
        {
            speed = 0f;
            curState = State.Stopped;
        }
        else
        {
            speed -= speedDecrease / motivation;
            moveSelf();
        }


    }

    public void kill()
    {
        speed = 0;
        isAlive = false;
        curState = State.Dead;
    }
    void generateMotivation()
    {
        motivation = Random.Range(1, 10);

    }

    void setColors()
    {

        switch (curState)
        {
            case State.TryingToMove:
                renderer.color = movingColor;
                break;
            case State.Stopped:
                renderer.color = stoppedColor;
                break;
            case State.Dead:
                renderer.color = deadColor;
                break;
            case State.Winner:
                renderer.color = victoryColor;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("EnemyEntered");
        if (other.tag == "FinishLine")
            curState = State.Winner;
    }
}
