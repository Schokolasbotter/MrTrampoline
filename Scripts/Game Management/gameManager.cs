using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    //References and Variables
    #region
    public enum GameState
    {
        start,
        trampoline,
        dance,
        pause,
        end
    };

    [Header("References")]
    public Transform threshold;
    public Transform character;
    public Camera cam;
    public GameObject trampoline;
    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;
    public GameObject startText;
    public GameObject barObject;
    private UIManager uiManager;
    private effectPlayer effectPlayer;
    private spriteManager characterSpriteManager;
    public discoScript discoScript;
    private musicManager musicManager;

    [Header("Game Settings")]
    public GameState gameState = GameState.start;
    [SerializeField] private int gameDifficulty = 0;
    public int totalScore = 0;
    public int danceScore = 0;
    public int totalDanceScore = 0;
    public float multiplier = 1.0f; 
    public int bonusCount = 1;
    public bool bonusState = false;
    public int scoreIncrement = 100;
    public int scoreThreshold1 = 10000;
    public int scoreThreshold2 = 20000;
    public int scoreThreshold3 = 30000;
    public int scoreThreshold4 = 40000;
    public int scoreThreshold5 = 50000;
    public int scoreThreshold6 = 60000;
    public int scoreThreshold7 = 70000;
    public int scoreThreshold8 = 80000;
    public int scoreThreshold9 = 90000;
    public int scoreThreshold10 = 100000;
    private bool isWrong = false;

    [Header("Camera Settings")]
    [SerializeField] private float trampolineSize = 5f;
    [SerializeField] private float dancingSize = 2f;
    [SerializeField] private Vector3 cameraPosition = new Vector3(0f, 1f, -10f);
    [SerializeField] [Range(0f,50f)] private float characterOffset;

    [Header("Dance Phase Settings")]
    [SerializeReference] private touchManager touchManager;
    private Arrow[] arrowQueue = new Arrow[3];

    [Header("Spawn Settings")]
    [SerializeReference] private spawnerScript spawner;
    [SerializeField] [Range(0.000001f, 0.1f)] private float bonusSpawnChance = 0.0001f;
    [SerializeField] [Range(0f, 20f)] private float bonusSpawnCooldownTimer = 2f;
    [SerializeField] private float bonusSpawnCooldown = 0f;
    [SerializeField] [Range(0.000001f, 0.1f)] private float obstacleSpawnChance = 0.0001f;
    [SerializeField] [Range(0f, 20f)] private float obstacleSpawnCooldownTimer = 2f;
    [SerializeField] private float obstacleSpawnCooldown = 0f;

    [Header("Screen Shake Settings")]
    [SerializeField] private float shakeIntensity = 0.5f;
    [SerializeField] private float shakeDuration = 0.5f;
    private Vector3 screenShakeOffset = Vector3.zero;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Get UI Manager
        uiManager = GetComponent<UIManager>();
        effectPlayer = GetComponent<effectPlayer>();
        musicManager = GetComponent<musicManager>();
        characterSpriteManager = character.GetComponent<spriteManager>();

        //Create Arrows
        createNewArrows(gameDifficulty);

        //Subscribe for Event
        touchManager.SwipeDetect += checkSwipe;
        touchManager.StartGame += startGame;
    }

    // Update is called once per frame
    void Update()
    {
        //Game Difficulty
        gameDifficulty = totalScore switch
        {
            int x when x < scoreThreshold1 => 0,
            int x when x < scoreThreshold2 => 1,
            int x when x < scoreThreshold3 => 2,
            int x when x < scoreThreshold4 => 3,
            int x when x < scoreThreshold5 => 4,
            int x when x < scoreThreshold6 => 5,
            int x when x < scoreThreshold7 => 6,
            int x when x < scoreThreshold8 => 7,
            int x when x < scoreThreshold9 => 8,
            int x when x >= scoreThreshold9 => 9,
            _ => 0
        };

        //Change GameState According to Character Position
        if (character.position.y >= threshold.position.y && gameState == GameState.trampoline && !isWrong)
        {
            changeGameState(GameState.dance);
        }
        else if(character.position.y < threshold.position.y && (gameState == GameState.dance || isWrong))
        {
            //Reset wrong swiped
            isWrong = false;
            //Change State
            changeGameState(GameState.trampoline);
        }

        //Camera
        followCharacter();

        //Rotate Arrows according to queue
        arrow1.transform.localEulerAngles = new Vector3(0f, 0f, arrowQueue[0]._rotationAngle);
        arrow2.transform.localEulerAngles = new Vector3(0f, 0f, arrowQueue[1]._rotationAngle);
        arrow3.transform.localEulerAngles = new Vector3(0f, 0f, arrowQueue[2]._rotationAngle);

        //Spawnbehaviour
        spawnObject(gameDifficulty);
    }

    public void changeGameState(GameState newGameState)
    {
        //Unpause
        Time.timeScale = 1;
        //Enable Trampoline
        trampoline.GetComponent<trampolineControlScript>().enabled = true;
        //Stop Pause
        uiManager.endPause();

        gameState = newGameState;

        if(gameState == GameState.trampoline)
        {
            //Activate Bar
            barObject.SetActive(true);
            //Change Camera Settings
            cam.gameObject.transform.position = cameraPosition;
            cam.orthographicSize = trampolineSize;
            //Show arrows
            cam.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //Add score to total score
            // Multiplier increases by bouncing
            // Bonuscount increases by getting bonusorbs
            if (bonusState)
            {
                totalDanceScore += (int)(danceScore * multiplier * bonusCount); // multiply with the bonusCound if player collected a bonusOrb
            }
            else
            {
                totalDanceScore += (int)(danceScore * multiplier); //use regular if no orb was collected
            }
            
            if(danceScore != 0)
            {
                StartCoroutine(AddScore());
            }
            bonusState = false;
            danceScore = 0;
            //Stop Disco
            discoScript.stopDisco();
            musicManager.startCircusMusic();
        }
        else if(gameState == GameState.dance)
        {
            //Deactivate Bar
            barObject.SetActive(false);
            //Change Camera Settings
            //Follow the character
            followCharacter();
            //Zoom in
            cam.orthographicSize = dancingSize;
            //Hide Arrows
            cam.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            //Create New Arrows
            createNewArrows(gameDifficulty);
            //Check for BonusOrbs
            if (spawner.isBonusOrbActive())
            {
                //Reset if bonus orb not taken
                bonusCount = 1;
            }
            //Destroy objects
            spawner.destroyObjects();
            //Start Disco
            discoScript.startDisco();
            musicManager.startDanceMusic();
        }
        else if(gameState == GameState.pause)
        {
            Time.timeScale = 0;
            trampoline.GetComponent<trampolineControlScript>().enabled = false;
            uiManager.startPause();
            
        }
        else if(gameState == GameState.end)
        {
            Time.timeScale = 0;
            totalScore += totalDanceScore;
            totalDanceScore = 0;
            trampoline.GetComponent<trampolineControlScript>().enabled = false;
            StartCoroutine(uiManager.endScreen());
        }
    }

    private void followCharacter()
    {
        if(gameState == GameState.dance)
        {
            //Follow the character
            Vector3 followCharacterpostion = new Vector3(character.position.x, character.position.y + characterOffset, -10f);
            cam.gameObject.transform.position = followCharacterpostion + screenShakeOffset;
            //Trampoline follow character
            trampoline.transform.position = new Vector3(character.position.x, trampoline.transform.position.y, trampoline.transform.position.z);
        }
    }

    public IEnumerator ShakeCameraDirection(Arrow arrow)
    {
        float elapsedTime = 0;
        while (elapsedTime < shakeDuration)
        {
            switch (arrow._direction)
            {
                default:
                    break;
                case Arrow.Direction.up:
                    //Add Up
                    screenShakeOffset.y -= Random.Range(0, 1f) * shakeIntensity;
                    break;
                case Arrow.Direction.down:
                    //Add down
                    screenShakeOffset.y += Random.Range(0, 1f) * shakeIntensity;
                    break;
                case Arrow.Direction.left:
                    //Add Left
                    screenShakeOffset.x += Random.Range(0, 1f) * shakeIntensity;
                    break;
                case Arrow.Direction.right:
                    //Add Left
                    screenShakeOffset.x -= Random.Range(0, 1f) * shakeIntensity;
                    break;

            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        screenShakeOffset = Vector3.zero;
    }

    public void checkSwipe()
    {
        if(gameState == GameState.dance)
        {
            //Create Arrow object
            float angle = touchManager.getAngle();
            Arrow performedArrow = new Arrow(angle);
            //Compare with first arrow in the list
            if(performedArrow._direction == arrowQueue[0]._direction)
            {
                //Correct
                //ScreenShake
                StartCoroutine(ShakeCameraDirection(performedArrow));
                //Sprite
                characterSpriteManager.setSprite(performedArrow);
                //score
                danceScore += scoreIncrement;
                //remove arrow, move every arrow up and create new arrow at the end
                arrowQueue[0] = arrowQueue[1];
                arrowQueue[1] = arrowQueue[2];
                arrowQueue[2] = new Arrow(gameDifficulty);
                effectPlayer.playCorrect();
            }
            else
            {
                isWrong = true;
                resetMultiplier();
                changeGameState(GameState.trampoline);
                effectPlayer.playFail();
                characterSpriteManager.setFallingSprite();
            }
        }
    }

    private void spawnObject(int difficulty)
    {
        if(gameState == GameState.trampoline)
        {
            if (difficulty >= 1) //Spawn Obstacle from difficulty level onwards
            {
                //Bonus Orb
                float randomValue = Random.value;
                bonusSpawnCooldown -= Time.deltaTime;
                float newBonusSpawnChance = bonusSpawnChance * difficulty; //Higher Change with higher difficulty level
                if (randomValue < newBonusSpawnChance && bonusSpawnCooldown <= 0f)
                {
                    bonusSpawnCooldown = bonusSpawnCooldownTimer; //Reset Cooldown
                    spawner.spawnBonusOrb(); //Spawn
                }
            }            

            if(difficulty >= 4) //Spawn Obstacle from difficulty level onwards
            {
                //Obstacle Orb
                float randomValue2 = Random.value;
                obstacleSpawnCooldown -= Time.deltaTime;
                if (randomValue2 < obstacleSpawnChance && obstacleSpawnCooldown <= 0f)
                {
                    obstacleSpawnCooldown = obstacleSpawnCooldownTimer; //Reset Cooldown
                    spawner.spawnObstacleOrb(); //Spawn
                    if(difficulty >= 9)
                    {
                        //Spawn more objects the further above you you are on the last threshold
                        int numberOfAdditionalObstacles = Mathf.RoundToInt((totalScore - scoreThreshold10) / 100000);
                        for(int i = 0; i < numberOfAdditionalObstacles; i++)
                        {
                            spawner.spawnObstacleOrb(); //Spawn
                        }
                    }
                }            
            }
        }
    }

    public void startGame()
    { 
        changeGameState(GameState.trampoline);
        startText.SetActive(false);
        character.GetComponent<bouncingBehaviour>().startBounce();
    }

    public void pauseGame()
    {
        if(gameState == GameState.pause)
        {
            changeGameState(GameState.trampoline);
        }
        else if(gameState != GameState.start)
        {
            changeGameState(GameState.pause);
        }        
    }

    public void endGame()
    {
        characterSpriteManager.setFallingSprite();
        changeGameState(GameState.end);
    }

    private void createNewArrows(int difficultyLevel)
    {
        //Add 3 new arrows
        arrowQueue[0] = new Arrow(difficultyLevel);
        arrowQueue[1] = new Arrow(difficultyLevel);
        arrowQueue[2] = new Arrow(difficultyLevel);
    }

    public void resetMultiplier()
    {
        multiplier = 1.0f;
        bonusCount = 0;
    }

    public void increaseMultiplier()
    {
        characterSpriteManager.setIdleSprite();
        multiplier += 0.1f;
    }

    public void activateBonus()
    {
        bonusState = true;
        bonusCount += 1;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator AddScore()
    {
        uiManager.totalDanceScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        effectPlayer.playScoreRoll();
        while(totalDanceScore > 0)
        {
            int decrementValue = (int) 0.01 * totalDanceScore;
            if(decrementValue < 10) { decrementValue = 10;}
            totalDanceScore -= decrementValue;
            totalScore += decrementValue;
            yield return new WaitForEndOfFrame();
        }
        effectPlayer.endScoreRoll();
        uiManager.totalDanceScore.gameObject.SetActive(false);
        effectPlayer.playScore();
    }

}
