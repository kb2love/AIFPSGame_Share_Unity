using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
    public static SceneMove sceneInst;
    void Awake()
    {
        /*if (sceneInst == null)
        {*/
            sceneInst = this;
        /*}
        else if (sceneInst != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);*/
    }
    public void TutoButton()
    {
        SceneManager.LoadScene("Tutorial Scene");
        DataManager.dataInstance.gameData.tutorial = true;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("BattleLoop Scene");
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
