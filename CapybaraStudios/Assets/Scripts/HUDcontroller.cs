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
    public GameObject tabMenuUI;


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

        if (Input.GetKey(KeyCode.Tab))
        {
            tabMenuUI.SetActive(true);
        }
        else
        {
            tabMenuUI.SetActive(false);
        }
    }

    public void Death()
    {
        deathMenuUI.GetComponent<CanvasGroup>().alpha = 0;
        deathMenuUI.SetActive(true);
        gameUI.SetActive(false);
        StartCoroutine(DeathFadein(1500));
    }


    private IEnumerator DeathFadein(float time)
    {
        if (time > 0)
        {
            yield return new WaitForSeconds(0.1f);
            deathMenuUI.GetComponent<CanvasGroup>().alpha += 0.06666666f;
            time -= 100;
            StartCoroutine(DeathFadein(time));
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        }
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