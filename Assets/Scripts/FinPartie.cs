using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinPartie : MonoBehaviour
{
    // Récupérer l'état du jeu et le score
    private int m_fin;
    private int m_score;

    [SerializeField] private GameObject m_panel;
    [SerializeField] private Text m_Text_Titre;
    [SerializeField] private Text m_Text_Score;

    private void Awake()
    {
        m_panel.SetActive(true);
        m_fin = PlayerPrefs.GetInt("Fin");
        m_score = PlayerPrefs.GetInt("Score");
    }

    void Start()
    {
        // Defaite
        if (m_fin == -1)
        {
            m_Text_Titre.text = "Defaite!!";
        }
        else if (m_fin == 1)
        {
            m_Text_Titre.text = "Victoire !!";
        }

        m_Text_Score.text = "Score = " + m_score;
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
