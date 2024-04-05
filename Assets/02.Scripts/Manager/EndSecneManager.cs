using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSecneManager : MonoBehaviour
{
    public Text[] scores; // 점수를 표시할 텍스트 배열
    private List<int> scored = new List<int>(); // 플레이어들의 점수를 저장할 리스트

    void Start()
    {
        scores = GameObject.Find("Panel-Score").GetComponentsInChildren<Text>(); // 텍스트 배열을 찾음

        // PlayerPrefs에서 "PlayerScores" 키를 사용하여 저장된 플레이어의 순위를 가져옴
        string scoresJson = PlayerPrefs.GetString("PlayerScores", "");
        if (!string.IsNullOrEmpty(scoresJson))
        {
            // 가져온 순위 데이터를 리스트로 변환
            scored = JsonUtility.FromJson<List<int>>(scoresJson);
        }

        // PlayerPrefs에서 "KillCount" 키를 사용하여 플레이어의 점수를 가져옴
        int _sc = PlayerPrefs.GetInt("KillCount");

        // 플레이어들의 점수 리스트에 가져온 점수 추가
        scored.Add(_sc);

        // 플레이어들의 점수 리스트를 내림차순으로 정렬
        scored.Sort((a, b) => b.CompareTo(a));

        // 정렬된 점수를 기반으로 각 텍스트에 순위와 점수를 표시
        for (int i = 0; i < scores.Length && i < scored.Count; i++)
        {
            scores[i].enabled = true;
            scores[i].text = (i + 1).ToString() + ". " + scored[i].ToString();
        }

        // 정렬된 플레이어들의 순위를 다시 PlayerPrefs에 저장
        string updatedScoresJson = JsonUtility.ToJson(scored);
        PlayerPrefs.SetString("PlayerScores", updatedScoresJson);

        // 기존에 저장한 "KillCount" 키 삭제
        PlayerPrefs.DeleteKey("KillCount");
    }
}
