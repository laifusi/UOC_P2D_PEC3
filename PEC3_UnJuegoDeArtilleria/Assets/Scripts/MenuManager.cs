using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    /// <summary>
    /// Method to load the Game scene
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Method to load the Menu scene
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Method to load the EndGame scene
    /// </summary>
    public void EndGame()
    {
        SceneManager.LoadScene("EndGame");
    }

    /// <summary>
    /// Method to exit the game
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
