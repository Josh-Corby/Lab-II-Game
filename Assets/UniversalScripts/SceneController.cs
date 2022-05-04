using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToTitleScene()
    {
        SceneManager.LoadScene("Main_Menu");
    }

 

    public void QuitGame()
    {
        Application.Quit();
    }
}
