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
        int scoreIdx = PlayerPrefs.GetInt("KillCount");
        for(int i = 0; i < scoreText.Length; i++)
        {
            if (scoreText[i].enabled == true) continue;
            scoreText[i].enabled = true;
            scoreText[i].text = i + 1 + ". " + scoreIdx.ToString();
            break;
        }
        GameManager.Instance.ScoreDelete();
    }
}