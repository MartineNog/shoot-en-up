using UnityEngine;

public class Sons : MonoBehaviour
{
    [Header("Source audio")]
    [SerializeField] private AudioSource m_Audio_Source;

    [Header("Musiques de jeu")]
    [SerializeField] private AudioClip m_Musique_Jeu;
    [SerializeField] private AudioClip m_Musique_Boss;
    [SerializeField] private AudioClip m_Musique_Bouclier;
    [SerializeField] private AudioClip m_Musique_Menu;
    
    [Header("Effets sonores")]
    [SerializeField] private AudioClip m_Effet_Impact_Joueur;
    [SerializeField] private AudioClip m_Effet_Impact_Ennemis;
    [SerializeField] private AudioClip m_Effet_Impact_Boule;
    [SerializeField] private AudioClip m_Effet_Impact_Bonus;


    private AudioClip m_Audio_Clip_Tmp;
    private bool enJeu = true;

    private void Awake()
    {
        // Lancer la musique du jeu
        m_Audio_Source.clip = m_Musique_Jeu;
        m_Audio_Source.Play();
    }

    void Update()
    {
        // Si le jeu est en pause
        if (Time.timeScale == 0)
        {
            // Si la musique n'a pas encore �t� lanc�e
            if (enJeu)
            {
                enJeu = false;

                // On r�cup�re quelle musique a �t� jou�e avant la pause
                m_Audio_Clip_Tmp = m_Audio_Source.clip;

                // On lance la musique de pause
                m_Audio_Source.clip = m_Musique_Menu;
                m_Audio_Source.Play();
            }
        }
        // Sinon
        else
        {
            if (enJeu == false)
            {
                enJeu = true;
                
                // On remet le son qui �t� mise avant la pause 
                m_Audio_Source.clip = m_Audio_Clip_Tmp;
                m_Audio_Source.Play();
            }
        }

        // Si c'est la musique du jeu qui doit �tre mise, sans qu'elle soit d�j� charg� et si le bouclier n'est pas activ�
        if (Player.player_S.Musique == 1 && Player.player_S.Changement && !Player.player_S.m_BonusShield)
        {
            // On lance la musique de jeu et on indique que le changement a �t� effectu�
            m_Audio_Source.clip = m_Musique_Jeu;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }
        // Si c'est la musique de jeu qui doit �tre lanc� mais que le bouclier est activ�
        else if(Player.player_S.Musique == 1 && Player.player_S.Changement && Player.player_S.m_BonusShield)
        {
            // On indique que la musique du jeu devra �tre lanc� lorsque le bonus sera fini
            Player.player_S.MusiqueAvant = 1;
        }

        // Si c'est la musique du boss qui doit �tre mise, sans qu'elle soit d�j� charg� et si me bouclier n'est pas activ�
        else if (Player.player_S.Musique == 2 && Player.player_S.Changement && !Player.player_S.m_BonusShield)
        {
            // On lance la nusique de jeu et on indique que le changment a �t� effectu�
            m_Audio_Source.clip = m_Musique_Boss;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }
        // Si c'est la musique du boss qui doit �tre lanc� mais que le bouclier est activ�
        else if (Player.player_S.Musique == 2 && Player.player_S.Changement && Player.player_S.m_BonusShield)
        {
            // On indique que la musique du boss devra �tre lanc� lorsque le bonus sera fini
            Player.player_S.MusiqueAvant = 2;
        }

        // Si c'est la musique du bouclier qui doit �tre lanc�, sans qu'elle soit d�j� charg�
        else if (Player.player_S.Musique == 3 && Player.player_S.Changement)
        {
            // On lance la musique du bouclier et on indique que le changement a �t� effectu�
            m_Audio_Source.clip = m_Musique_Bouclier;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }

        // Si le joueur s'est pris un coup 
        if (Player.player_S.EffetSonore == 1)
        {
            // On lance l'effet sonore associ�
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Joueur);
            Player.player_S.EffetSonore = 0;
        }
            
        // Si un ennemie est touch�
        if (Player.player_S.EffetSonore == 2)
        {
            // On lance l'effet sonore associ�
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Ennemis);
            Player.player_S.EffetSonore = 0;
        }

        // S'il y a un contact boule � boule de neige
        if (Player.player_S.EffetSonore == 3)
        {
            // On lance l'effet sonore associ�
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Boule);
            Player.player_S.EffetSonore = 0;
        }

        // Si un bonus a �t� r�cup�r�
        if (Player.player_S.EffetSonore == 4)
        {
            // On lance l'effet sonore associ�
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Bonus);
            Player.player_S.EffetSonore = 0;
        }
    }
}
