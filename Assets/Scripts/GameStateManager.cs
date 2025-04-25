using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public PlayerRecord Player;
    public PlayerRecord BestPlayer;

    private readonly string saveFileName = "saveRecordlist.json";
    private RecordList recordList;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayerData();
    }

    [Serializable]
    public class PlayerRecord
    {
        public string Name;
        public int BestScore;
    }

    [Serializable]
    public class RecordList
    {
        public List<PlayerRecord> saveDataList = new List<PlayerRecord>();

        public string latestPlayerName = "";

        public int Count()
        {
            return saveDataList.Count;
        }

        public PlayerRecord BestPlayer()
        {
            PlayerRecord bestPlayerRecord = null;
            int bestScore = 0;

            foreach (var record in saveDataList)
            {
                if (record.BestScore > bestScore)
                {
                    bestScore = record.BestScore;
                    bestPlayerRecord = record;
                }
            }

            return bestPlayerRecord;
        }

        public PlayerRecord FindLastPlayer()
        {
            PlayerRecord lastPlayerRecord = null;
            int bestScore = 0;

            foreach (var record in saveDataList)
            {
                if (record.Name.Equals(latestPlayerName) && record.BestScore >= bestScore)
                {
                    bestScore = record.BestScore;
                    lastPlayerRecord = record;
                }
            }

            return lastPlayerRecord;
        }

        public PlayerRecord FindOrCreatePlayer(string playerName)
        {
            PlayerRecord player = null;
            foreach (var record in saveDataList)
            {
                if (record.Name.Equals(playerName))
                {
                    player = record;
                }
            }

            if (player == null)
            {
                player = new PlayerRecord();
                player.Name = playerName;
                player.BestScore = 0;
                saveDataList.Add(player);
            }

            latestPlayerName = player.Name;
            return player;
        }
    }

    public void LoadPlayerData()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            recordList = JsonUtility.FromJson<RecordList>(jsonData);
            BestPlayer = recordList.BestPlayer();
            Player = recordList.FindLastPlayer();
        }
        else
        {
            recordList = new RecordList();
            BestPlayer = null;
            Player = null;
        }
    }

    public void SaveRecordsData()
    {
        string jsonData = JsonUtility.ToJson(recordList);
        string saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(saveFilePath, jsonData);
    }
    
    public PlayerRecord FindOrCreatePlayer(string playerName)
    {
        Player = recordList.FindOrCreatePlayer(playerName); ;
        return Player;
    }
}
