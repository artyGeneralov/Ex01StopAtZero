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
    Color movingColor = Color.green;
    Color stoppingColor = Color.yellow;
    Color deadColor = Color.red;
    Color victoryColor = Color.magenta;
    private SpriteRenderer renderer;
    private float speed, lastPressTime;
    private KeyCode lastKey;
    private bool isAlive;
    private bool canMove;
    private bool hasWon = false;
    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        speed = 0f;
        lastKey = 0f;
        lastKey = KeyCode.A;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (hasWon)
        {
            setColor(victoryColor);

        }

        if (!canMove)
            return;
        calculateFriction();


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
        if (speed < 0.007f) speed = 0;
         
        if (speed == 0)
        {
            setColor(stoppingColor);

        }
        else
        {
            setColor(movingColor);
        }

        transform.Translate(Vector3.up * speed * Time.deltaTime);

    }

    void calculateFriction()
    {
        float logSpeed = Mathf.Log10(speed+1);
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
        renderer.color = color;
    }

    public void setMove(bool move)
    {
        if (!isAlive)
            return;
        canMove = move;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player Entered");
        if (other.tag == "FinishLine")
            hasWon = true;
    }

    void victory()
    {
        setMove(false);
        setColor(victoryColor);
    }

}
