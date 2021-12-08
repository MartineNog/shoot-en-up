using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private bool m_isPause = false;
    [SerializeField] private GameObject m_Pause_Menu;
    // Start is called before the first frame update
    void Start()
    {
        m_isPause = false;
        Time.timeScale = 1f;
    }

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

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
