using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSecneManager : MonoBehaviour
{
    public Text[] scores;
    private List<Text> scoreList = new List<Text>();
    private List<int> scored = new List<int>();
    void Start()
    {
        scores = GameObject.Find("Panel-Score").GetComponentsInChildren<Text>();
        for(int i = 0;  i < scores.Length; i++)
        {
            scoreList.Add(scores[i]);
        }
        scoreList.RemoveAt(0);
        scored.Add(PlayerPrefs.GetInt("KillCount"));
        for(int i = 0;i < scores.Length; i++)
        {
            if (scoreList[i].enabled == true) continue;
            scoreList[i].enabled = true;
            for(int j = 0; j < scored.Count; j++)
            {

                scoreList[i].text = "1. " + scored[j].ToString();
            }
            if (scoreList[i].enabled == true) break;
        }
        PlayerPrefs.DeleteKey("KillCount");
    }
}
