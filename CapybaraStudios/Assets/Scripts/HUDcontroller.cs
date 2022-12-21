using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HUDcontroller : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public GameObject gameUI;


    void Start()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
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

    public void Death()
    {
        deathMenuUI.SetActive(true);
        gameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        //TODO if bedingung, nur wenn Singleplayer, dann timeScale
        Time.timeScale = 1;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        //TODO if bedingung, nur wenn Singleplayer, dann timeScale
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        //TODO if bedingung, nur wenn Singleplayer, dann timeScale
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Respawn()
    {
        FindObjectOfType<GameManager>().Respawn();
    }
}