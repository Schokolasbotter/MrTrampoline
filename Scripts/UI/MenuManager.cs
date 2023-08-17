using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region
    [Header("References")] [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject offlineLeaderboardMenu, onlineLeaderboardMenu, howToMenu, creditsMenu;
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private offlineLeaderBoard offlineLeaderBoard;
    [SerializeField] private leaderboardManager leaderboardManager;
    [SerializeField] private UIManager UIManager;
    [Header("Variables")]
    public string onlineRank;
    public string offlineRank;
    #endregion

    //Menu functions
    public void startGame()
    {
        //Loads Next Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void goToMenu()
    {
        //Load First Scene
        SceneManager.LoadScene(0);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator GetNewRanks(int score)
    {
        //Get Positions of both
        //Online
        onlineRank = "";
        leaderboardManager.GetLeaderboardRank(score);
        yield return new WaitUntil(() => leaderboardManager.onlineRank != "");
        onlineRank = leaderboardManager.onlineRank;
        //Offline
        offlineRank = "";
        //Go through scores
        for (int i = 0; i < offlineLeaderBoard.scores.Length; i++)
        {
            //If smaller oer equal continue
            if (offlineLeaderBoard.scores[i] >= score)
            {
                if(offlineLeaderBoard.names[i] != "")
                {
                    continue;
                }
                else
                {
                    offlineRank = (i + 1).ToString();
                    break;
                }
            }
            //if bigger get the rank
            else if (offlineLeaderBoard.scores[i] < score)
            {
                offlineRank = (i + 1).ToString();
                break;
            }
        }
        //if board is full (10 scores)
        if (offlineRank == "")
        {
            offlineRank = "No New Rank";
        }
    }

    public IEnumerator SubmitScore(int score)
    {
        //CLear player prefs
        PlayerPrefs.DeleteAll();
        //Get the name from Inputfield
        string playerName = inputText.text;
        print("Player: " + playerName + " Score: " + score);
        //Load into Online Leaderboard
        leaderboardManager.SetLeaderboardEntry(playerName, score);
        // Put into offline Leaderboard
        for (int i = offlineLeaderBoard.scores.Length - 1; i >= 0; i--)
        {
            //Check for empty positions
            if (offlineLeaderBoard.names[i] == "" && i != 0)
            {
                continue;
            }

            if (score <= offlineLeaderBoard.scores[i])
            {
                if (i == 9)
                {
                    break;
                }
                else
                {
                    offlineLeaderBoard.scores[i + 1] = score;
                    offlineLeaderBoard.names[i + 1] = playerName;
                    break;
                }
            }
            else
            {
                if (i == 9)
                {
                    continue;
                }
                else if (i == 0)
                {
                    offlineLeaderBoard.scores[i + 1] = offlineLeaderBoard.scores[i];
                    offlineLeaderBoard.names[i + 1] = offlineLeaderBoard.names[i];
                    offlineLeaderBoard.scores[i] = score;
                    offlineLeaderBoard.names[i] = playerName;
                    continue;
                }
                else
                {
                    offlineLeaderBoard.scores[i + 1] = offlineLeaderBoard.scores[i];
                    offlineLeaderBoard.names[i + 1] = offlineLeaderBoard.names[i];
                    continue;
                }
            }
        }
        yield return new WaitUntil(() => leaderboardManager.uploadFinished == true);
        UIManager.finishUpload(leaderboardManager.uploadSuccessful);
        leaderboardManager.uploadFinished = false;
        print("Finished Submitting Scores");
    }

    #region Menu Management

    public void goToOnlineLeaderboard()
    {
        leaderboardManager.GetLeaderboard();
        MainMenu.SetActive(false);
        onlineLeaderboardMenu.SetActive(true);
    }
    public void goToOfflineLeaderboard()
    {
        leaderboardManager.getOfflineLeaderboard();
        MainMenu.SetActive(false);
        offlineLeaderboardMenu.SetActive(true);
    }

    public void goToHowTo()
    {
        MainMenu.SetActive(false);
        howToMenu.SetActive(true);
    }

    public void goToCredits()
    {
        MainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void backToMenu()
    {
        creditsMenu.SetActive(false);
        howToMenu.SetActive(false);
        onlineLeaderboardMenu.SetActive(false);
        offlineLeaderboardMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion
}
