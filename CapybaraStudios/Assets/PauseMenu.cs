using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        //TODO if bedingung, nur wenn Singleplayer, dann timeScale
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        //TODO if bedingung, nur wenn Singleplayer, dann timeScale
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        //TODO if bedingung, nur wenn Singleplayer, dann timeScale
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu_Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}