using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    [Header("Healthbar")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image HealthbarTotal;
    [SerializeField] private Image HealthbarCurrent;

    [Header ("Manabar")]
    [SerializeField] private PlayerAttack playerMana;
    [SerializeField] private Image ManaTotal;
    [SerializeField] private Image ManaCurrent;

    [Header("ExpBar")]
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private Image ExperienceTotal;
    [SerializeField] private Image ExperienceCurrent;
    [SerializeField] private TextMeshProUGUI LevelText;


    private void Awake()
    {
        HealthbarTotal.fillAmount = 1;
        ManaTotal.fillAmount = 1;
        ExperienceTotal.fillAmount = 1;        
    }

    private void Update()
    {
        //Heath
        HealthbarCurrent.fillAmount = playerHealth.CurrentHealth / playerHealth.MaximumHealth;
        //Mana
        ManaCurrent.fillAmount = playerMana.CurrentMana / playerMana.MaximumMana;

        //Level & Experience;
        ExperienceCurrent.fillAmount = playerLevel.currentExperience / playerLevel.experienceToNextLevel;
        LevelText.text = "Level:" + playerLevel.GetCurrentLevel().ToString();
               
    }

}
