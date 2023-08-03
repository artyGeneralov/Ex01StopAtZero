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

    private SpriteRenderer renderer;
    private float speed, lastPressTime;
    private KeyCode lastKey;
    private bool isAlive;
    private bool canMove;
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
            renderer.color = stoppingColor;

        }
        else
        {
            renderer.color = movingColor;
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
        isAlive = false;
        setMove(false);
        renderer.color = deadColor;
    }

    public void setMove(bool move)
    {
        canMove = move;
    }

}
