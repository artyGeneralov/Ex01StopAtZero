using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMng : MonoBehaviour
{

    enum States
    {
        START,
        COUNTING_HIDDEN,
        COUNTING_FINAL,
        EVALUATE_MOVEMENT,
        EVALUATION,
        VICTORY_SCREEN
    }
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] TMPro.TextMeshProUGUI victoryTextBox;
    [SerializeField] TMPro.TextMeshProUGUI guideTextBox;
    [SerializeField] TMPro.TextMeshProUGUI smallGuideTextBox;
    int numberOfEnemies = 10;
    [SerializeField] float spaceBetweenEnemies = 1f;
    float initialX = -8f;
    TimeTextManager timeManager;
    PlayerMovement playerMovement;
    List<GameObject> listOfEnemies;
    List<GameObject> listOfWinners;
    [SerializeField] GameObject restartButton;
    int numberOfWinners;
    int maxNumberOfWinners = 2;

    float yOfEnemy = -4.64f;
    bool isUpdateReady = false;
    States currentState;

    void Start()
    {
        // sets random times
        victoryTextBox.alpha = 0f;
        currentState = States.START;
        playerMovement = Object.FindObjectOfType<PlayerMovement>();
        playerMovement.setMove(false);
        listOfEnemies = new List<GameObject>();
        listOfWinners = new List<GameObject>();
        numberOfWinners = 0;
        instantiateEnemies();
        forAllEnemies(script => script.setMoving(false));
        // Instantiate all enemies
        StartCoroutine(DelayedStart());
        guideTextBox.alpha = 1f;
        smallGuideTextBox.alpha = 1f;


    }



    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(setTimeWhenReady());
        isUpdateReady = true;
        guideTextBox.alpha = 0.1f;
        smallGuideTextBox.alpha = 0f;
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

    void Update()
    {
        if (currentState == States.VICTORY_SCREEN)
            return;
        if(numberOfWinners >= maxNumberOfWinners)
        {
            //victory screen
            victoryScreen();

            return; // no more state changes
        }


        if (isUpdateReady)
        {
            int curTime = timeManager.getCurrentTime();

            switch (currentState)
            {
                case States.START:
                    //Empty for now
                    break;
                case States.COUNTING_HIDDEN:
                    if(!playerMovement.hasWon()) playerMovement.setMove(true);
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

                    killAllMoving();
                    Invoke("setNewTime", 3.0f);
                    currentState = States.EVALUATION;
                    if (!playerMovement.hasWon())  playerMovement.setMove(false);
                    break;
                case States.EVALUATION:
                    //Empty for now
                    break;
                default:
                    break;
            }
        }
    }


    void victoryScreen()
    {
        // kill everyone who didnt win yet
        currentState = States.VICTORY_SCREEN;
        killAllMoving();
        killAllStanding();
        // create victory text
        StringBuilder victoryText = new StringBuilder();
        victoryText.Append("Winners:\n");
        foreach(GameObject entity in listOfWinners)
        {
            switch (entity.tag)
            {
                case "Enemy":
                    int id = entity.GetComponent<EnemyMovement>().getId();
                    victoryText.Append("* Enemy " + id + "\n");
                    break;
                case "Player":
                    victoryText.Append("* Player\n");
                    break;
            }
        }
        victoryTextBox.text = victoryText.ToString();
        victoryTextBox.alpha = 1f;


        Instantiate(restartButton);
    }


    void killAllMoving()
    {
        if (playerMovement.isMoving() && (!playerMovement.hasWon()))
            playerMovement.killPlayer();
        forAllEnemies(script =>
        {
            if (script.isMoving() && (!script.hasWon()))
            {
                script.kill();
            }
        });
    }

    void killAllStanding()
    {
        if (!playerMovement.hasWon())
            playerMovement.killPlayer();
        forAllEnemies(script =>
        {
            if (!script.hasWon())
            {
                script.kill();
            }
        });
    }


    void setNewTime()
    {
        int randTime = Random.Range(5, 9);
        timeManager.setCurTime(randTime);
        currentState = States.COUNTING_HIDDEN;

        // signal enemies to move
        forAllEnemies(script =>
        {
            if(!script.hasWon())
                script.setMoving(true);
            });

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
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (i == 4)
                continue;
            float xOfEnemy = initialX + (i * spaceBetweenEnemies);
            GameObject instance = Instantiate(enemyPrefab,
                                                new Vector3(xOfEnemy, yOfEnemy, 0), Quaternion.identity);
            instance.GetComponent<EnemyMovement>().setId(i);
            listOfEnemies.Add(instance);
        }
    }

    public void notifyOfVictory(GameObject winner)
    {
        if (!listOfWinners.Contains(winner))
        {
            listOfWinners.Add(winner);
            numberOfWinners++;
        }
    }

    public void restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}
