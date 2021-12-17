using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private AudioSource m_Audio_Source;
    [SerializeField] private AudioClip m_Musique_Menu;

    private void Awake()
    {
        m_Audio_Source.clip = m_Musique_Menu;
        m_Audio_Source.Play();
    }

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
