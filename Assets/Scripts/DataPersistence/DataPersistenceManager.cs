using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }
    public static event Action postLoad;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Persistence Manager");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        Invoke("LoadGame", 0.1f);
        ScoreManager.onLevelUp += OnLevelUp;
        PlantingManager.instance.onSeedsGained +=OnSeedsGained;
        PlantingManager.instance.onPlantPlanted += OnPlantPlanted;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No save, starting new game");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(this.gameData);
        }

        postLoad?.Invoke(); // Invoke postLoad event after loading is complete
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnLevelUp(int level)
    {
        SaveGame();
    }
    private void OnSeedsGained(PlantSO plant)
    {
        SaveGame();
    }
    private void OnPlantPlanted()
    {
        SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    #if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause()
    {
        SaveGame();
    }
    #endif

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
