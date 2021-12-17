using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sons : MonoBehaviour
{
    [SerializeField] private AudioSource m_Audio_Source;
    [SerializeField] private AudioClip m_Musique_Jeu;
    [SerializeField] private AudioClip m_Musique_Boss;
    [SerializeField] private AudioClip m_Musique_Bouclier;
    [SerializeField] private AudioClip m_Musique_Menu;

    [SerializeField] private AudioClip m_Effet_Impact_Joueur;
    [SerializeField] private AudioClip m_Effet_Impact_Ennemis;
    [SerializeField] private AudioClip m_Effet_Impact_Boule;
    [SerializeField] private AudioClip m_Effet_Impact_Bonus;

    private AudioClip m_Audio_Clip_Tmp;

    private bool enJeu = true;

    private void Awake()
    {
        m_Audio_Source.clip = m_Musique_Jeu;
        m_Audio_Source.Play();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            if (enJeu)
            {
                enJeu = false;
                m_Audio_Clip_Tmp = m_Audio_Source.clip;
                m_Audio_Source.clip = m_Musique_Menu;
                m_Audio_Source.Play();
            }
        }
        else
        {
            if (enJeu == false)
            {
                enJeu = true;
                m_Audio_Source.clip = m_Audio_Clip_Tmp;
                m_Audio_Source.Play();
            }
            

            
        }

        if (Player.player_S.Musique == 1 && Player.player_S.Changement)
        {
            m_Audio_Source.clip = m_Musique_Jeu;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }
        else if (Player.player_S.Musique == 2 && Player.player_S.Changement)
        {
            m_Audio_Source.clip = m_Musique_Boss;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }
        else if (Player.player_S.Musique == 3 && Player.player_S.Changement)
        {
            m_Audio_Source.clip = m_Musique_Bouclier;
            m_Audio_Source.Play();
            Player.player_S.Changement = false;
        }

        if (Player.player_S.EffetSonore == 1)
        {
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Joueur);
            Player.player_S.EffetSonore = 0;
        }
            

        if (Player.player_S.EffetSonore == 2)
        {
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Ennemis);
            Player.player_S.EffetSonore = 0;
        }

        if (Player.player_S.EffetSonore == 3)
        {
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Boule);
            Player.player_S.EffetSonore = 0;
        }

        if (Player.player_S.EffetSonore == 4)
        {
            m_Audio_Source.PlayOneShot(m_Effet_Impact_Bonus);
            Player.player_S.EffetSonore = 0;
        }
    }
}
