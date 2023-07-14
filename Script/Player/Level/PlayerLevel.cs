using UnityEngine;

public class PlayerLevel : MonoBehaviour
{

    [SerializeField] public int experienceToNextLevel;
    public int currentExperience;
    private int currentLevel;

    private Animator anim;
    private Health playerHeath;
    private PlayerAttack playerAttack;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerHeath = GetComponent<Health>();
        playerAttack = GetComponent<PlayerAttack>();


        //Load the save data from Save Manager
        GameSaveData saveData = SaveManager.instance.GetSaveData();

        if (saveData != null)
        {
            currentLevel = saveData.playerCurrentLevel;
            currentExperience = saveData.playerCurrentExperience;
            experienceToNextLevel = saveData.playerExperienceToNextLevel;
        }
        else
        {
            currentLevel = 1;
            currentExperience = 0;
            experienceToNextLevel = 100;
        }
    }

    //Subcribe to an event
    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;

        Debug.Log("Subcribe to an event!");
    }

    //Unsubcribing from an event
    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;

        Debug.Log("Unsubcribing from an event!");
    }

    private void HandleExperienceChange(int newExperience)
    {
        currentExperience += newExperience;
        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    private void LevelUp()
    {
        anim.SetTrigger("win");

        //Leveled up
        currentLevel++;

        //Increased player's stat
        playerHeath.MaximumHealth += 200;
        playerAttack.MaximumMana += 20;
        playerAttack.damage += 40;
        
        //Fillup player's Health & Mana
        playerHeath.CurrentHealth = playerHeath.MaximumHealth;
        playerAttack.CurrentMana = playerAttack.MaximumMana;


        //Return currentExp and set new value of requiredExp
        currentExperience -= experienceToNextLevel;
        experienceToNextLevel *= 2;

        // Perform level up actions, such as increasing stats or unlocking new abilities

        Debug.Log("Level Up! Level: " + currentLevel);

        //Save the game data
        SaveManager.instance.Save();
    }
}
