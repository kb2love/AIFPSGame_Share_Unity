using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSecneManager : MonoBehaviour
{
    [SerializeField] private Text[] scoreText;
    void Start()
    {
        scoreText = GameObject.Find("Panel-Score").GetComponentsInChildren<Text>();
        List<int> scoreIdx = DataManager.dataInstance.gameData.score;
        scoreIdx.Sort((a, b) => b.CompareTo(a));
        for (int i = 0; i < scoreIdx.Count; i++)
        {
            if (scoreText.Length < i | scoreIdx.Count < i) return;
            scoreText[i].enabled = true;
            scoreText[i].text = i + 1 + ". " + scoreIdx[i].ToString();
        }
    }
}