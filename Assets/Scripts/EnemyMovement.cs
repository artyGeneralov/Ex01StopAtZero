using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    int id;
    int motivation; // 1 to 10
    [SerializeField] float baseSpeed = 0.3f;
    [SerializeField] float speedDecrease = 0.008f;
    [SerializeField] GameObject idText;
    float speed;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float speedEpsilon = 0.015f;
    float timer = 0f;
    [SerializeField] float delay = 0.5f;
    GameMng gameManager;

    Color movingColor = Color.green;
    Color stoppedColor = Color.cyan;
    Color deadColor = Color.red;
    Color victoryColor = Color.magenta;
    new SpriteRenderer renderer;
    TimeTextManager timeManager;
    bool isAlive;
    float finalStoppingTime;
    enum State
    {
        TryingToMove,
        TryingToStop,
        Stopped,
        Dead,
        Winner,
        Done
    }
    State curState;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameMng>();
        curState = State.Stopped;
        generateMotivation();
        renderer = GetComponent<SpriteRenderer>();
        speed = baseSpeed;
        isAlive = true;
        timeManager = Object.FindObjectOfType<TimeTextManager>();
        finalStoppingTime = Random.Range(6000, 12000);
        finalStoppingTime /= 1000;
        speedEpsilon = (float)Random.Range(100, 999) / 10000f;
    }
    public void setId(int id)
    {
        this.id = id+1;
        idText.GetComponent<TMPro.TextMeshPro>().text = this.id.ToString();
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
                gameManager.notifyOfVictory(gameObject);
                stopSelf();
                break;
            default:
                break;
        }
        setColors();

    }


    public void setMoving(bool move)
    {
        if (move == false)
            curState = State.Stopped;
        else
            curState = State.TryingToMove;
    }
    void moveSelf()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    void calculateMovement()
    // we randomize a number, wherever it falls in this graph - this is what we would do.
    // 
    // <---|----|--->
    //  s    n    f
    {
        speed = baseSpeed;
        int rand = Random.Range(1, 11);
        int slowBound = Mathf.Clamp(10 - motivation, 1, 10); // motivation 10 - slow bound = 1 // motivation 1 - slow bound - 9
        int fastBound = Mathf.Clamp(1 + motivation, 1, 10);
        if (rand <= slowBound)
        {

            speed -= speedEpsilon;

        }
        else if (rand >= fastBound)
        {
            speed += speedEpsilon;
          
        }

        speed = Mathf.Clamp(speed, 0, maxSpeed);

        // decision to start stopping?
        if (timeManager.getCurrentTime() <= 2)
            if (rand >= Mathf.Clamp((10 - motivation), 1, 8))
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
        if (curState != State.Winner)
        {
            if (speed <= 0.005f)
            {
                speed = 0f;
                curState = State.Stopped;
            }
            else
            {
                speed = Mathf.Min(0, speed - speedDecrease / motivation);
                moveSelf();
            }
        }
        else // In case of victory - Final stop!!
        {
            if (speed <= 0.005f)
            {
                speed = 0f;
                curState = State.Done;
            }
            else
            {
                speed -= speedDecrease / finalStoppingTime;
                moveSelf();
            }
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

        if (other.tag == "FinishLine")
            curState = State.Winner;
    }

    public bool hasWon()
    {
        if (curState >= State.Winner)
            return true;
        return false;
    }
    public bool isDead()
    {
        return !isAlive;
    }
    public int getId()
    {
        return this.id;
    }
}
