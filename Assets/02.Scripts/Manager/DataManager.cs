using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.iOS;
public class GameData
{
    public string ranking;
    public int[] score;
    public bool tutorial;
}
public class DataManager : MonoBehaviour
{
    public static DataManager dataInstance;
    public GameData gameData = new GameData();
    private string path;
    private string fileName = "GameData";
    void Awake()
    {
        if(dataInstance == null)
        {
            dataInstance = this;
        }
        else if(dataInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath + "/";
    }
    public void SaveData()
    {
        string data = JsonUtility.ToJson(gameData);
        File.WriteAllText(path + fileName, data);
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + fileName);
        gameData =  JsonUtility.FromJson<GameData>(data);
    }
}
