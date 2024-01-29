using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void LoadSpaceLevel()
    {
        SceneManager.LoadScene(3);
    }
    
    public static void LoadArenaLevel()
    {
        SceneManager.LoadScene(2);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
