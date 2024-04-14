using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.iOS;
public class GameData
{
    public string ranking;
    public List<int> score = new List<int>();
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
        path = Path.Combine(Application.persistentDataPath, fileName);
    }
    public void SaveData()
    {
        string data = JsonUtility.ToJson(gameData);
        File.WriteAllText(path, data);
    }
    public void LoadData()
    {
        if(File.Exists(path))
        {

            string data = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<GameData>(data);
        }
    }
}
