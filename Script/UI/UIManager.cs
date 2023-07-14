using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("GameOver")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseScreen;

    [Header("OptionsMenu")]
    [SerializeField] private GameObject optionsScreen;


    private bool gameoverisActive;
    private bool isSetting;

    private void Awake()
    {
        //gameoverScreen
        gameOverScreen.SetActive(false);
        gameoverisActive = false;

        //pauseScreen
        pauseScreen.SetActive(false);

        //optionsScreen
        optionsScreen.SetActive(false);
        isSetting = false;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);

        gameoverisActive = true;
    }

    private void Update()
    {
        if (gameoverisActive = true && Input.anyKey)
        {
            gameOverScreen.SetActive(false);
            gameoverisActive = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If pause screen is active -> unpause
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }

        if (isSetting)
            PauseGame(false);
        else return;
    }
    #region PauseMenu
    public void PauseGame(bool status)
    {
        //If status = true -> pause the game || If status == false -> unpause
        pauseScreen.SetActive(status);

        //When pause status is true -> change the timescale to 0 (pause the game)
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    //Loading MainMenu Screen
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exits play mode
#endif
    }

    public void Options()
    {
        optionsScreen.SetActive(true);

        isSetting = true;

        if (isSetting)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    #endregion

    #region OptionsMenu
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    } 

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

    public void Back()
    {
        optionsScreen.SetActive(false);
    }
    #endregion

    #region MainMenu

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    #endregion
}
