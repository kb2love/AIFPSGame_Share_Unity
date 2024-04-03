using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
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
    }
        
}
