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

    [SerializeField] private AudioSource m_Audio_Source;
    [SerializeField] private AudioClip m_Musique_Victoire;
    [SerializeField] private AudioClip m_Musique_Defaite;
    [SerializeField] private AudioClip m_Musique_Menu;
 

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
            m_Audio_Source.PlayOneShot(m_Musique_Defaite);
            StartCoroutine(AttenteMusiqueDefaite());
        }
        else if (m_fin == 1)
        {
            m_Text_Titre.text = "Victoire !!";
            m_Audio_Source.PlayOneShot(m_Musique_Victoire);
            StartCoroutine(AttenteMusiqueVictoire());
        }

        m_Text_Score.text = "Score = " + m_score;
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator AttenteMusiqueDefaite()
    {
        yield return new WaitForSeconds(3f);
        m_Audio_Source.clip = m_Musique_Menu;
        m_Audio_Source.Play();
    }

    IEnumerator AttenteMusiqueVictoire()
    {
        yield return new WaitForSeconds(2f);
        m_Audio_Source.clip = m_Musique_Menu;
        m_Audio_Source.Play();
    }
}
