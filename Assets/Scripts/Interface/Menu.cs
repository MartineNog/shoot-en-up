using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Texte du meilleur score")]
    [SerializeField] private Text m_High_Score;

    [Header("Different panel du menu")]
    [SerializeField] private GameObject m_Panel_Menu;
    [SerializeField] private GameObject m_Panel_Commandes;
    [SerializeField] private GameObject m_Panel_Credits;

    [Header("Son")]
    [SerializeField] private AudioSource m_Audio_Source;
    [SerializeField] private AudioClip m_Musique_Menu;

    private void Awake()
    {
        // Affichage du meilleur score
        m_High_Score.text = "High Score = " + PlayerPrefs.GetInt("HighScore");

        // Activer le panel principale du menu
        m_Panel_Menu.SetActive(true);

        // Desactiver les panel de crédit et des commandes
        m_Panel_Commandes.SetActive(false);
        m_Panel_Credits.SetActive(false);

        // Lancement de la musique du menu
        m_Audio_Source.clip = m_Musique_Menu;
        m_Audio_Source.Play();
    }

    // A l'appui du bouton Play
    public void PlayGame()
    {
        // On charge le jeu
        SceneManager.LoadScene(1);
    }

    // A l'appui du bouton quitter
    public void QuitGame()
    {
        // On quitte l'application
        Application.Quit();
    }

    // A l'appui du bouton des commandes
    public void GameControl()
    {
        // Désactiver le panel principale pour activer celui des commandes
        m_Panel_Menu.SetActive(false);
        m_Panel_Commandes.SetActive(true);
    }

    // A l'appui du bouton des credits
    public void Credit()
    {
        // Désactiver le panel principale pour activer le panel des crédits
        m_Panel_Menu.SetActive(false);
        m_Panel_Credits.SetActive(true);
    }

    // A l'appui du bouton pour fermer les panels
    public void QuitPanel()
    {
        // On active le panel principale en désactivant les 2 autres
        m_Panel_Menu.SetActive(true);
        m_Panel_Commandes.SetActive(false);
        m_Panel_Credits.SetActive(false);
    }
}
