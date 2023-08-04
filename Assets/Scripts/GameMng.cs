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
    [SerializeField] GameObject enemyPrefab;
    int numberOfEnemies = 10;
    float spaceBetween = 1f;
    float initialX = -8f;
    TimeTextManager timeManager;
    PlayerMovement playerMovement;
    List<GameObject> listOfEnemies;

    float yOfEnemy = -4.64f;
    States currentState;
    // Start is called before the first frame update
    void Start()
    {
        // sets random times

        currentState = States.START;
        playerMovement = Object.FindObjectOfType<PlayerMovement>();
        StartCoroutine(setTimeWhenReady());
        listOfEnemies = new List<GameObject>();
        instantiateEnemies();
        // Instantiate all enemies


    }

    private IEnumerator setTimeWhenReady()
    {
        while (timeManager == null)
        {
            timeManager = UnityEngine.Object.FindObjectOfType<TimeTextManager>();
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

                // sound ?

                break;
            case States.EVALUATE_MOVEMENT:
                Debug.Log("EVALUATE_MOVEMENT STATE");

                if (playerMovement.isMoving())
                    playerMovement.killPlayer();
                forAllEnemies(script => {
                    if (script.isMoving())
                    {
                        script.kill();
                    }
                });
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

        // signal enemies to move
        forAllEnemies(script => script.setMoving(true));

    }

    void forAllEnemies(System.Action<EnemyMovement> action)
    {
        foreach (GameObject enemy in listOfEnemies)
        {
            if (enemy != null)
            {
                EnemyMovement script = enemy.GetComponent<EnemyMovement>();
                if (script != null)
                    action(script); // actionscript lol
            }
        }
        removeEmptyEnemies();
    }

    void removeEmptyEnemies()
    {
        for (int i = 0; i < listOfEnemies.Count; i++)
            if (listOfEnemies[i] == null)
                listOfEnemies.RemoveAt(i);
    }

    void instantiateEnemies()
    {
        for(int i = 0; i < numberOfEnemies; i++)
        {
            float xOfEnemy = initialX + (i * spaceBetween);
            GameObject instance = Instantiate(enemyPrefab,
                                                new Vector3(xOfEnemy, yOfEnemy, 0), Quaternion.identity);
            listOfEnemies.Add(instance);
            
        }
    }


}
