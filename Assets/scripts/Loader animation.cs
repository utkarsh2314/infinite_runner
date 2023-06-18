using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loaderanimation : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void MainScene ()
    {
        StartCoroutine(LoadLevel(0));
    }
    public void GameScene ()
    {
        StartCoroutine(LoadLevel(1));
    }
    public void about_page ()
    {
        StartCoroutine(LoadLevel(2));
    }
    public void quit_page ()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
        
    }
}
