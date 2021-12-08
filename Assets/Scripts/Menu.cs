using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // A l'appui du bouton Play
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // A l'appui du bouton quitter
    public void QuitGame()
    {
        Application.Quit();
    }

}
