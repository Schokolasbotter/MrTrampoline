using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OfflineLeaderBoard", menuName = "ScriptableObjects/Offline Leader Board", order = 1)]
public class offlineLeaderBoard:ScriptableObject
{
    public string[] names = { "", "", "", "", "", "", "", "", "", "" };
    public int[] scores = {0,0,0,0,0,0,0,0,0,0};
}
