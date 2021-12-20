using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinPartie : MonoBehaviour
{
    // Récupérer l'état du jeu et le score
    private int m_fin;
    private int m_score;
    private int m_high_score;

    [Header("Panel de fin de partie")]
    [SerializeField] private GameObject m_panel;

    [Header("Affichage du panel de fin")]
    [SerializeField] private Text m_Text_Titre;
    [SerializeField] private Text m_Text_Score;

    [Header("Renard et bonhomme de neige en fonction de la victoire ou de la défaite")]
    [SerializeField] private GameObject m_Fox_Win;
    [SerializeField] private GameObject m_Fox_Fail;
    [SerializeField] private GameObject m_Snowman_Win;
    [SerializeField] private GameObject m_Snowman_Fail;

    [Header("Musiques")]
    [SerializeField] private AudioSource m_Audio_Source;
    [SerializeField] private AudioClip m_Musique_Victoire;
    [SerializeField] private AudioClip m_Musique_Defaite;
    [SerializeField] private AudioClip m_Musique_Menu;
 

    private void Awake()
    {
        // Activer le panel
        m_panel.SetActive(true);

        // Récupération de l'état de la partie
        m_fin = PlayerPrefs.GetInt("Fin");

        // Récupération du score et le comparer au meilleur score
        m_score = PlayerPrefs.GetInt("Score");
        m_high_score = PlayerPrefs.GetInt("HighScore");
        if (m_score >= m_high_score)
        {
            PlayerPrefs.SetInt("HighScore", m_score);
        }

        // Desactiver tous les renard et bonhomme de neige de décoration
        m_Fox_Win.SetActive(false);
        m_Fox_Fail.SetActive(false);
        m_Snowman_Win.SetActive(false);
        m_Snowman_Fail.SetActive(false);
    }

    void Start()
    {
        // Si le jeu s'est conclue par une defaite
        if (m_fin == -1)
        {
            // On active le renard et le bonhomme de neige associé à la défaite
            m_Fox_Fail.SetActive(true);
            m_Snowman_Fail.SetActive(true);

            // On met à jour le titre de la page de fin
            m_Text_Titre.text = "GAME OVER...";
            m_Text_Titre.color = new Color(125, 0, 0, 255);

            // On lance la musique de la défaite
            m_Audio_Source.PlayOneShot(m_Musique_Defaite);

            // Puis on lance la musique par défaut de ce panel
            StartCoroutine(AttenteMusiqueDefaite());
        }
        // Si le jeu s'est conclue par une victoire
        else if (m_fin == 1)
        {
            // On active le renard et le bonhomme de neige associé à la victoire
            m_Fox_Win.SetActive(true);
            m_Snowman_Win.SetActive(true);

            // On met a jour le titre de la page de fin
            m_Text_Titre.text = "CONGRATULATION !!";
            m_Text_Titre.color = new Color(0, 125, 0, 255);

            // On lance la musique de la victoire 
            m_Audio_Source.PlayOneShot(m_Musique_Victoire);

            // Puis on lance la musique par défaut de ce panel
            StartCoroutine(AttenteMusiqueVictoire());
        }

        // On affiche le score obtenu
        m_Text_Score.text = "Score = " + m_score;
    }
    
    // A l'appui du bouton du lenu principale
    public void ReturnToMenu()
    {
        // On charge le menu
        SceneManager.LoadScene(0);
    }

    // A l'appui du bouton reset
    public void ResetLevel()
    {
        // On charge la partie
        SceneManager.LoadScene(1);
    }

    // Attendre pendant la durée de la musique de la défaite
    IEnumerator AttenteMusiqueDefaite()
    {
        // Mettre en pause la fonction en attendant que la musique de la défait se lance
        yield return new WaitForSeconds(3f);

        // Lancement de la musque par défaut du panel
        m_Audio_Source.clip = m_Musique_Menu;
        m_Audio_Source.Play();
    }
    // Attendre pendant la durée de la musique de la victoire
    IEnumerator AttenteMusiqueVictoire()
    {
        // Mettre en pause la fonction en attendant que la musique de la victoire se lance
        yield return new WaitForSeconds(2f);

        // Lancement de la musque par défaut du panel
        m_Audio_Source.clip = m_Musique_Menu;
        m_Audio_Source.Play();
    }
}
