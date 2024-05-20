using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    
    private void Start()
    {
        DataManager.dataInstance.LoadData();
        if (DataManager.dataInstance.gameData.tutorial)
            transform.GetChild(1).gameObject.SetActive(false);
    }
    public void TutoScene()
    {
        SceneMove.sceneInst.TutoButton();
    }
    public void StartGame()
    {
        SceneMove.sceneInst.PlayGame();
    }
    public void QuitGame()
    {
        SceneMove.sceneInst.QuitButton();
        DataManager.dataInstance.SaveData();
    }
        
}
