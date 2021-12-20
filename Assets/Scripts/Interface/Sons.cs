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
            // Si la musique n'a pas encore été lancée
            if (enJeu)
            {
                enJeu = false;

                // On récupère quelle musique a été jouée avant la pause
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
                
                // On remet le son qui été mise avant la pause 
                m_Audio_Source.clip = m_Audio_Clip_Tmp;
                m_Audio_Source.Play();
            }
        }

        // Si c'est la musique du jeu qui doit être mise, sans qu'elle soit déjà chargé et si le bouclier n'est pas activé
        if (Player.player_S.Musique == 1 && Player.player_S.Changement && !Player.player_S.m_BonusShield)
        {
            // On lance la musique de jeu et on indique que le changement a été effectué
            m_Audio_Source.clip = m_Musique_Jeu;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }
        // Si c'est la musique de jeu qui doit être lancé mais que le bouclier est activé
        else if(Player.player_S.Musique == 1 && Player.player_S.Changement && Player.player_S.m_BonusShield)
        {
            // On indique que la musique du jeu devra être lancé lorsque le bonus sera fini
            Player.player_S.MusiqueAvant = 1;
        }

        // Si c'est la musique du boss qui doit être mise, sans qu'elle soit déjà chargé et si me bouclier n'est pas activé
        else if (Player.player_S.Musique == 2 && Player.player_S.Changement && !Player.player_S.m_BonusShield)
        {
            // On lance la nusique de jeu et on indique que le changment a été effectué
            m_Audio_Source.clip = m_Musique_Boss;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }
        // Si c'est la musique du boss qui doit être lancé mais que le bouclier est activé
        else if (Player.player_S.Musique == 2 && Player.player_S.Changement && Player.player_S.m_BonusShield)
        {
            // On indique que la musique du boss devra être lancé lorsque le bonus sera fini
            Player.player_S.MusiqueAvant = 2;
        }

        // Si c'est la musique du bouclier qui doit être lancé, sans qu'elle soit déjà chargé
        else if (Player.player_S.Musique == 3 && Player.player_S.Changement)
        {
            // On lance la musique du bouclier et on indique que le changement a été effectué
            m_Audio_Source.clip = m_Musique_Bouclier;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }

        // Si le joueur s'est pris un coup 
        if (Player.player_S.EffetSonore == 1)
        {
            // On lance l'effet sonore associé
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Joueur);
            Player.player_S.EffetSonore = 0;
        }
            
        // Si un ennemie est touché
        if (Player.player_S.EffetSonore == 2)
        {
            // On lance l'effet sonore associé
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Ennemis);
            Player.player_S.EffetSonore = 0;
        }

        // S'il y a un contact boule à boule de neige
        if (Player.player_S.EffetSonore == 3)
        {
            // On lance l'effet sonore associé
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Boule);
            Player.player_S.EffetSonore = 0;
        }

        // Si un bonus a été récupéré
        if (Player.player_S.EffetSonore == 4)
        {
            // On lance l'effet sonore associé
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Bonus);
            Player.player_S.EffetSonore = 0;
        }
    }
}
