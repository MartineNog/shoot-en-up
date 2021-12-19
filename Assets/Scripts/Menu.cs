using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject m_Panel_Menu;
    [SerializeField] private GameObject m_Panel_Commandes;
    [SerializeField] private GameObject m_Panel_Credits;

    [SerializeField] private AudioSource m_Audio_Source;
    [SerializeField] private AudioClip m_Musique_Menu;

    private void Awake()
    {
        m_Panel_Menu.SetActive(true);
        m_Panel_Commandes.SetActive(false);
        m_Panel_Credits.SetActive(false);

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

    // A l'appui du bouton des commandes
    public void GameControl()
    {
        m_Panel_Menu.SetActive(false);
        m_Panel_Commandes.SetActive(true);
    }

    // A l'appui du bouton des credits
    public void Credit()
    {
        m_Panel_Menu.SetActive(false);
        m_Panel_Credits.SetActive(true);
    }

    // A l'appui du bouton pour fermer les panels
    public void QuitPanel()
    {
        m_Panel_Menu.SetActive(true);
        m_Panel_Commandes.SetActive(false);
        m_Panel_Credits.SetActive(false);
    }
}
