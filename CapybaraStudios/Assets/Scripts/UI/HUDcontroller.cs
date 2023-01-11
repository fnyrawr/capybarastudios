using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HUDcontroller : MonoBehaviour
{
    private static bool _gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public GameObject gameUI;
    public GameObject tabMenuUI;


    void Start()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void DoPause()
    {
        if (_gameIsPaused)
        {
            Resume();
        }
        else
        {
            tabMenuUI.SetActive(false);
            Pause();
        }
    }

    public void Tab()
    {
        if (!_gameIsPaused)
        {
            tabMenuUI.SetActive(tabMenuUI.activeSelf);
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
        _gameIsPaused = false;
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
        _gameIsPaused = true;
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