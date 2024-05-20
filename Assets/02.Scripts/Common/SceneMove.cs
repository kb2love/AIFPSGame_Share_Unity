using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
    public static SceneMove sceneInst;
    void Awake()
    {
        sceneInst = this;
    }
    public void TutoButton()
    {
        SceneManager.LoadScene(1);
        DataManager.dataInstance.gameData.tutorial = true;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }
    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void EndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
    public void QuitButton()
    {
        DataManager.dataInstance.SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
