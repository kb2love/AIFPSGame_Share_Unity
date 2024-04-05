using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSecneManager : MonoBehaviour
{
    public Text[] scores; // ������ ǥ���� �ؽ�Ʈ �迭
    private List<int> scored = new List<int>(); // �÷��̾���� ������ ������ ����Ʈ

    void Start()
    {
        scores = GameObject.Find("Panel-Score").GetComponentsInChildren<Text>(); // �ؽ�Ʈ �迭�� ã��

        // PlayerPrefs���� "PlayerScores" Ű�� ����Ͽ� ����� �÷��̾��� ������ ������
        string scoresJson = PlayerPrefs.GetString("PlayerScores", "");
        if (!string.IsNullOrEmpty(scoresJson))
        {
            // ������ ���� �����͸� ����Ʈ�� ��ȯ
            scored = JsonUtility.FromJson<List<int>>(scoresJson);
        }

        // PlayerPrefs���� "KillCount" Ű�� ����Ͽ� �÷��̾��� ������ ������
        int _sc = PlayerPrefs.GetInt("KillCount");

        // �÷��̾���� ���� ����Ʈ�� ������ ���� �߰�
        scored.Add(_sc);

        // �÷��̾���� ���� ����Ʈ�� ������������ ����
        scored.Sort((a, b) => b.CompareTo(a));

        // ���ĵ� ������ ������� �� �ؽ�Ʈ�� ������ ������ ǥ��
        for (int i = 0; i < scores.Length && i < scored.Count; i++)
        {
            scores[i].enabled = true;
            scores[i].text = (i + 1).ToString() + ". " + scored[i].ToString();
        }

        // ���ĵ� �÷��̾���� ������ �ٽ� PlayerPrefs�� ����
        string updatedScoresJson = JsonUtility.ToJson(scored);
        PlayerPrefs.SetString("PlayerScores", updatedScoresJson);

        // ������ ������ "KillCount" Ű ����
        PlayerPrefs.DeleteKey("KillCount");
    }
}
