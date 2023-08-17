using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //References and variables
    #region
    [Header("UI References")]
    public GameObject UICanvas;
    public TextMeshProUGUI score;
    public TextMeshProUGUI totalDanceScore;
    public TextMeshProUGUI multiplier;
    public GameObject danceCanvas;
    public TextMeshProUGUI danceScore;
    public TextMeshProUGUI danceMultiplier;
    public TextMeshProUGUI danceBonusMultiplier;
    public GameObject pauseCanvas;
    public TextMeshProUGUI pauseScore;
    public TextMeshProUGUI pauseMultiplier;
    public GameObject endCanvas, submitScoreGameobject, gettingRanksGameobject, displayRanksGameobject, submittedGameobject;
    public TextMeshProUGUI endScore;
    public TextMeshProUGUI onlineLeaderboardText;
    public TextMeshProUGUI onlineLeaderboard;
    public TextMeshProUGUI localLeaderboardText;
    public TextMeshProUGUI localLeaderboard;
    public GameObject newOnlineRank;
    public GameObject newLocalRank;
    public MenuManager menuManager;

    [Header("Script References")]
    private gameManager gameManager;

    // Start is called before the first frame update
    #endregion
    void Start()
    {
        //Get Manager Script
        gameManager = GetComponent<gameManager>();

        //Update Text
        //General UI
        score.text = gameManager.totalScore.ToString();
        multiplier.text = "x" + string.Format("{0:F1}", gameManager.multiplier);
        totalDanceScore.text = gameManager.totalDanceScore.ToString();
        //Dance UI
        danceScore.text = gameManager.danceScore.ToString();
        danceMultiplier.text = "x" + string.Format("{0:F1}", gameManager.multiplier);
        danceBonusMultiplier.text = "x" + string.Format("{0:F1}", gameManager.bonusCount);
    }

    // Update is called once per frame
    void Update()
    {
        //Bonus Multiplier
        if (gameManager.bonusState)
        {
            danceBonusMultiplier.gameObject.SetActive(true);
        }
        else
        {
            danceBonusMultiplier.gameObject.SetActive(false);
        }

        //Acivate Canvas if in dance phase
        if(gameManager.gameState == gameManager.GameState.dance)
        {

            danceCanvas.SetActive(true);
        }
        else
        {
            danceCanvas.SetActive(false);
        }

        //General UI
        score.text = gameManager.totalScore.ToString();
        multiplier.text = "x" + string.Format("{0:F1}", gameManager.multiplier);
        totalDanceScore.text = gameManager.totalDanceScore.ToString();
        //Dance UI
        danceScore.text = gameManager.danceScore.ToString();
        danceMultiplier.text = "x" + string.Format("{0:F1}", gameManager.multiplier);
        danceBonusMultiplier.text = "x" + string.Format("{0:F1}", gameManager.bonusCount);
    }

    public void startPause()
    {
        UICanvas.SetActive(false);
        pauseScore.text = gameManager.totalScore.ToString();
        pauseMultiplier.text = string.Format("{0:F1}", gameManager.multiplier);
        pauseCanvas.SetActive(true);
    }

    public void endPause()
    {
        pauseCanvas.SetActive(false);
        UICanvas.SetActive(true);
    }

    public IEnumerator endScreen()
    {
        //Show UI
        //Update Texts
        endScore.text = gameManager.totalScore.ToString();
        //Activate Objects
        gettingRanksGameobject.SetActive(true);
        displayRanksGameobject.SetActive(false);
        submitScoreGameobject.SetActive(false);
        submittedGameobject.SetActive(false);
        UICanvas.SetActive(false);
        endCanvas.SetActive(true);
        //Get Leaderbboard
        StartCoroutine(menuManager.GetNewRanks(gameManager.totalScore));
        //Wait Until Ranks updated
        yield return new WaitUntil(() => menuManager.onlineRank != "" && menuManager.offlineRank != "");
        //Update Rank Texts
        //Online
        if(menuManager.onlineRank.Equals("No New Rank"))
        {
            onlineLeaderboard.text = "No New Rank ";
            newOnlineRank.SetActive(false);            
        }
        else
        {
            onlineLeaderboard.text = "Rank " + menuManager.onlineRank;
            newOnlineRank.SetActive(true);
        }
        //Offline
        if(menuManager.offlineRank == "No New Rank")
        {
            localLeaderboard.text = "No New Rank";
            newLocalRank.SetActive(false);
        }
        else
        {
            localLeaderboard.text = "Rank " + menuManager.offlineRank;
            newLocalRank.SetActive(true);
        }
        //Update Objects
        gettingRanksGameobject.SetActive(false);
        displayRanksGameobject.SetActive(true);
        submitScoreGameobject.SetActive(true);
        //Reset Ranks
        menuManager.onlineRank = "";
        menuManager.offlineRank = "";
    }
    
    public void submitScore()
    {
        StartCoroutine(menuManager.SubmitScore(gameManager.totalScore));
    }

    public void finishUpload(bool uploadState)
    {
        if (uploadState) {
            submittedGameobject.GetComponent<TextMeshProUGUI>().text = "Submitted";
            submitScoreGameobject.SetActive(false);
            submittedGameobject.SetActive(true);
        }
        else
        {
            submittedGameobject.GetComponent<TextMeshProUGUI>().text = "Couldn't Submit";
            submitScoreGameobject.SetActive(false);
            submittedGameobject.SetActive(true);
        }
    }
}
