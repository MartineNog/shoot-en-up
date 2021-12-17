using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] float m_Enemy_Speed;
    [SerializeField] private float m_margin;

    // Bonus
    [SerializeField] private GameObject m_Bonus_SpeedPlayer;
    [SerializeField] private GameObject m_Bonus_CadenceBalle;
    [SerializeField] private GameObject m_Bonus_SpeedBalle;
    [SerializeField] private GameObject m_BonusShield;

    private int m_PV_enemies = 5;

    private void Awake()
    {
        WriteCurrentPV(m_PV_enemies);
        m_MainCamera = Camera.main;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si l'ennemi a touché le joueur il disparait
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject, 0.3f);
        }

        // Si l'ennemi est touché par un projectile, il perd de la vie
        if (collision.gameObject.tag == "Bullet")
        {
            WriteCurrentPV(ReducePV(6));

            if (ReadCurrentPV() <= 0)
            {
                int rand = Random.Range(0, 20);     // Calculer un nombre aléatoire pour savoir s'il donne un bonus ou non
                if (rand == 4)  // Instancier le bonus augmentant la vitesse du joueur
                {
                    m_Bonus_SpeedPlayer.transform.position = transform.position;
                    Instantiate(m_Bonus_SpeedPlayer);
                }
                else if (rand == 8) // Instancier le bonus du bouclier du joueur 
                {
                    m_BonusShield.transform.position = transform.position;
                    Instantiate(m_BonusShield);
                }
                else if (rand == 12) // Instancier le bonus augmentant la cadence de tir
                {
                    m_Bonus_CadenceBalle.transform.position = transform.position;
                    Instantiate(m_Bonus_CadenceBalle);
                }
                else if (rand == 16) // Instancier le bonus augmentant la force de tir
                {
                    m_Bonus_SpeedBalle.transform.position = transform.position;
                    Instantiate(m_Bonus_SpeedBalle);
                }

                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si le projectile touche le bouclier
        if (other.gameObject.tag == "Shield")
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (m_MainCamera.WorldToScreenPoint(transform.position).y > (0 - m_margin))
        {
            transform.position -= Vector3.up * Time.deltaTime * m_Enemy_Speed;
        }
        else if (m_MainCamera.WorldToScreenPoint(transform.position).y <= (0 - m_margin))
        {
            Destroy(this.gameObject);
        }
    }
}
