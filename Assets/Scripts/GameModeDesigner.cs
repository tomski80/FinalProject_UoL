using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeDesigner : MonoBehaviour
{
    public void LoadGameLevel()
    {
        Debug.Log("Load Game Level");
        SceneManager.LoadScene("GameplayLevel", LoadSceneMode.Single);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
