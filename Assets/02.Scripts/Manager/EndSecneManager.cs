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
        int[] scoreIdx = DataManager.dataInstance.gameData.score;
        for(int i = 0; i < scoreText.Length; i++)
        {
            for(int j = 0; j < scoreIdx.Length; j++)
            {
                if (scoreText[i].enabled == true) continue;
                scoreText[i].enabled = true;
                scoreText[i].text = i + 1 + ". " + scoreIdx[j].ToString();
                break;
            }
            break;
        }
        GameManager.Instance.ScoreDelete();
    }
}