using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    private GameSaveData saveData;

    /*
    [SerializeField] Health playerHealth;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] PlayerLevel playerLevel;
    [SerializeField] PlayerRespawn playerRespawn;
    */

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Load()
    {
        string filePath = Application.persistentDataPath + "/playerInfo.dat";

        if (File.Exists(filePath))
        {
            // Open the file stream and create a BinaryFormatter to deserialize the save data
            FileStream file = File.Open(filePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            // Deserialize the save data from the file
            saveData = (GameSaveData)bf.Deserialize(file);
            file.Close();



        }
        else
        {
            // If the save file doesn't exist, create a new empty save data object
            saveData = new GameSaveData();
        }
    }

    public void Save()
    {
        saveData = new GameSaveData();

        Health playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        PlayerAttack playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        PlayerLevel playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevel>();
        PlayerRespawn playerRespawn = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRespawn>();

        saveData.playerMaximumHealth = playerHealth.MaximumHealth;
        saveData.playerMaximumMana = playerAttack.MaximumMana;
        saveData.playerDamage = playerAttack.damage;
        saveData.playerCurrentLevel = playerLevel.GetCurrentLevel();
        saveData.playerCurrentExperience = playerLevel.currentExperience;
        saveData.playerExperienceToNextLevel = playerLevel.experienceToNextLevel;
        saveData.playerCheckpointPosition = playerRespawn.currentCheckpoint.position;
        
        /*
        // Create a BinaryFormatter and file stream to serialize the save data to a file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        // Serialize the save data to the file
        bf.Serialize(file, saveData);
        file.Close();
        */

    }
    public void DeleteSaveData()
    {
        string filePath = Application.persistentDataPath + "/playerInfo.dat";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public GameSaveData GetSaveData()
    {
        return saveData;
    }

}

[System.Serializable]
public class GameSaveData
{
    public float playerMaximumHealth;
    public float playerMaximumMana;
    public float playerDamage;
    public int playerCurrentLevel;
    public int playerCurrentExperience;
    public int playerExperienceToNextLevel;

    public Vector3 playerCheckpointPosition;
    // Add any other game data you want to save
}