using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    private static ExperienceManager instance;
    public static ExperienceManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Create a new instance if it doesn't exist
                GameObject obj = new GameObject("ExperienceManager");
                instance = obj.AddComponent<ExperienceManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }


    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler OnExperienceChange;


    public void AddExperience (int amount)
    {
        OnExperienceChange?.Invoke(amount);
    }
}
