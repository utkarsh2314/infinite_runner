using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool isPaused=false;

    void Start()
    {

        //Debug.Log("Quit");
        pauseMenu.SetActive(false);
    }
    
    void Update()
    {

        //Debug.Log("Quit");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }
    }
    public void ReplayGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        Debug.Log("1");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseGame()
    {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
            isPaused = false;
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


}