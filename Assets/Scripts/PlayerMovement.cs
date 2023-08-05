using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float speedDecreaseFactor = 1.5f;
    [SerializeField] private float friction = 0.9f;
    [SerializeField] private float maxFriction = 0.997f;
    [SerializeField] private float minFriction = 0.8f;
    [SerializeField] private float finalStoppingSpeed = 1.01f;
    [SerializeField] GameMng gameManager;
    enum States
    {
        ForceStopped,
        Stopped,
        Moving,
        Victory,
        Done
    }
    Color movingColor = Color.green;
    Color stoppingColor = Color.yellow;
    Color deadColor = Color.red;
    Color victoryColor = Color.magenta;
    Color currentColor;
    private SpriteRenderer renderer;
    private float speed, lastPressTime;
    private KeyCode lastKey;
    private bool isAlive;
    private bool won = false;
    // Start is called before the first frame update
    States curState;

    void Start()
    {
        isAlive = true;
        speed = 0f;
        lastKey = 0f;
        lastKey = KeyCode.A;
        renderer = GetComponent<SpriteRenderer>();
        curState = States.Stopped;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        switch (curState)
        {
            case States.Stopped:
                setColor(stoppingColor);
                playerMovementCotroller();
                break;
            case States.Moving:
                setColor(movingColor);
                playerMovementCotroller();
                break;
            case States.Victory:
                setColor(victoryColor);
                victory();
                break;
            case States.Done:
                break;
        }

    }

    void playerMovementCotroller()
    {

        if (won)
            return;
        calculateFriction();
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            curState = States.Moving;
        }

        // Player Movement by alternating A & D
        if (Input.GetKeyDown(KeyCode.A) && lastKey == KeyCode.D)
        {
            lastKey = KeyCode.A;
            if (Time.time - lastPressTime < 1f)
                speed = baseSpeed / (Time.time - lastPressTime);
            lastPressTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastKey == KeyCode.A)
        {
            lastKey = KeyCode.D;
            if (Time.time - lastPressTime < 1f)
                speed = baseSpeed / (Time.time - lastPressTime);
            lastPressTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastKey == KeyCode.D ||
                Input.GetKeyDown(KeyCode.A) && lastKey == KeyCode.A)
        {
            speed /= speedDecreaseFactor;
        }
        else
        {
            speed *= friction;
        }
        if (speed < 0.007f && (Time.time - lastPressTime > 0.5f)) speed = 0f;

        if (speed == 0f && !hasWon())
        {
            curState = States.Stopped;
        }
        else if(speed != 0f && !hasWon())
        {
            curState = States.Moving;
        }

        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void calculateFriction()
    {
        float logSpeed = Mathf.Log10(speed + 1);
        friction = minFriction + (maxFriction - minFriction) * logSpeed;
        friction = Mathf.Clamp(friction, minFriction, maxFriction);
    }
    public bool isMoving()
    {
        if (speed != 0)
            return true;
        return false;
    }
    public void killPlayer()
    {
        setMove(false);
        isAlive = false;
        setColor(deadColor);
    }

    void setColor(Color color)
    {
        if (currentColor != color)
            renderer.color = color;
        currentColor = color;
    }

    public void setMove(bool move)
    {
        if (isAlive == false)
            return;
        if (move == false)
            curState = States.ForceStopped;
        else
            curState = curState == States.ForceStopped ? States.Stopped : curState;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FinishLine")
        {
            setMove(false);
            curState = States.Victory;
        }
        
    }

    void victory()
    {
        // notify Manager
        gameManager.notifyOfVictory(gameObject);

        speed /= finalStoppingSpeed;
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (speed == 0)
            curState = States.Done;
    }

    public bool hasWon()
    {
        if (curState >= States.Victory)
            return true;
        return false;
    }

}
