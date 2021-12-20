using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [Header("Panel de pause")]
    [SerializeField] private bool m_isPause = false;
    [SerializeField] private GameObject m_Pause_Menu;
   
    void Start()
    {
        // On désactive le menu de pause et on lance le jeu
        m_isPause = false;
        Time.timeScale = 1f;
    }

    // A l'appui du bouton de pause
    public void BePause()
    {
        // Si on était en pause, on désactive le menu de pause et on relance le jeu
        if (m_isPause)
        {
            m_Pause_Menu.SetActive(false);
            Time.timeScale = 1f;
        }
        // Si on était en jeu, on active le menu de pause et on arrete le jeu
        else
        {
            m_Pause_Menu.SetActive(true);
            Time.timeScale = 0f;
        }

        // On inverse l'état de la pause
        m_isPause = !m_isPause;
    }

    // A l'appui du bouton accueil
    public void ReturnToMenu()
    {
        // On relance le jeu et on charge la pase de menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
