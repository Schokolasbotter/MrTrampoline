using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.Events;

public class leaderboardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> entries;
    [SerializeField] private List<GameObject> offlineEntries;
    public List<TextMeshProUGUI> ranks;
    public List<TextMeshProUGUI> names;
    public List<TextMeshProUGUI> scores;
    public List<TextMeshProUGUI> offlineRanks;
    public List<TextMeshProUGUI> offlineNames;
    public List<TextMeshProUGUI> offlineScores;
    public offlineLeaderBoard offlineLeaderBoard;
    public bool uploadSuccessful = false, uploadFinished = false;
    public string onlineRank = "";

    private void Awake()
    {
        //Get all TMP Elements at wakeup
        for (int i = 0; i < entries.Count; i++)
        {
            ranks.Add(entries[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            names.Add(entries[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>());
            scores.Add(entries[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>());
        }
        for (int i = 0; i < offlineEntries.Count; i++)
        {
            offlineRanks.Add(offlineEntries[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            offlineNames.Add(offlineEntries[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>());
            offlineScores.Add(offlineEntries[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>());
        }

    }
    
    private void Start()
    {
        //Get Leaderboard
        GetLeaderboard();
    }

    private string publicLeaderboardKey = "a2220e71993b06ad817c668725df837e81680b3d3a8e7eaf2d76e6325c2b001c";
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => {
            int loopLength = (msg.Length < entries.Count) ? msg.Length : entries.Count;
            for(int i = 0; i < loopLength; i++)
            {
                entries[i].SetActive(true);
                //Fill up all available ranks
                ranks[i].text = (i + 1).ToString();
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void getOfflineLeaderboard()
    {
        for(int i = 0; i < offlineLeaderBoard.scores.Length; i++)
        {
            if(offlineLeaderBoard.names[i] != "")
            {
                offlineEntries[i].SetActive(true);
                offlineRanks[i].text = (i + 1).ToString();
                offlineNames[i].text = offlineLeaderBoard.names[i];
                offlineScores[i].text = offlineLeaderBoard.scores[i].ToString();
            }
        }
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        string extra = SystemInfo.deviceUniqueIdentifier;
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, extra, (msg) =>
            {
                if (msg) { print("Upload Successful"); }
                uploadSuccessful = msg;
                uploadFinished = true;
            });
    }

    public void GetLeaderboardRank(int score)
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => {

            int entries = msg.Length;
            for (int i = 0; i < msg.Length; i++)
            {
                //Maximum 100 entries
                if (i == 100)
                {
                    break;
                }
                //Go trough scores
                //Same or smaller continue
                if(msg[i].Score >= score)
                {
                    continue;
                }
                //Bigger get the rank it will be placed in
                else if(msg[i].Score < score)
                {
                    onlineRank = (i+1).ToString();
                    break;
                }                
            }
            //If no ranking
            if(onlineRank == "" )
            {
                //Full Board
                if(entries >= 100)
                {
                    onlineRank = "No New Rank";
                }
                //Leaderboard is not full yet
                else
                {
                    onlineRank = (entries+1).ToString();
                }
            }

        }));
    }    
}
