using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    private GameData gameData;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if(gameData == null)
        {
            Debug.Log("No data was found, Initializing data to defaults.");
            NewGame();
        }
    }

    public void SaveGame()
    {

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
