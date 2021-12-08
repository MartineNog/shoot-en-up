using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_enemy;
    [SerializeField] private float m_Time_Enemies = 3;

    private bool vague = false;
    private int nVague;
    int compteurVague = 3;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    void Start()
    {
        StartCoroutine(Enemies());
    }

    IEnumerator Enemies()
    {
        int compteur = 0;
        while (Application.isPlaying)
        {
            float borne = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).x;

            if (compteurVague <= 0 && !vague)
            {
                
                vague = true;
                nVague = Mathf.RoundToInt(Random.Range(0.6f, 3.4f));
                print($"Vague {nVague}");
                compteur = 0;
            }

            if (vague)
            {
                m_Time_Enemies = 0.3f;
                WaveMove(borne, compteur);
                compteur++;
            }
            else
            {
                m_Time_Enemies = 3;
                RandomMove(borne);
                compteurVague--;
            }
            yield return new WaitForSeconds(m_Time_Enemies);
        }   
    }

    private void RandomMove(float borne)
    {
        float position_x = Random.Range(-borne, borne);    // A mettre en aléatoire
        float position_y = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).y;  // A mettre en haut de l'écran
        float position_z = m_player.transform.position.z;   // Profondeur du joueur

        transform.position = new Vector3(position_x, position_y, position_z);
        m_enemy.transform.position = transform.position;
        Instantiate(m_enemy);
    }

    private void WaveMove(float borne, int compteur)
    {
        float position_x = 0;
        float position_y = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).y;  // A mettre en haut de l'écran
        float position_z = m_player.transform.position.z;   // Profondeur du joueur

        // Deplacement en vague
        if (nVague == 1)
        {
            position_x = (-borne) + compteur * 1.5f;
            if (position_x >= borne)
            {
                vague = false;
                compteurVague = (int)Random.Range(5, 10);
                print("Fini");
            }

            transform.position = new Vector3(-position_x, position_y, position_z);
            m_enemy.transform.position = transform.position;
            Instantiate(m_enemy);
        }
        // Déplacement en ligne vers la droite
        else if (nVague == 2)
        {
            position_x = (-borne) + compteur * 2;
            if (position_x >= borne)
            {
                vague = false;
                compteurVague = (int)Random.Range(5, 10);
                print("Fini");
            }
        }
        // Déplacement en ligne vers la gauche
        else if (nVague == 3)
        {
            position_x = borne - compteur * 2;
            if (position_x <= -borne)
            {
                vague = false;
                compteurVague = (int)Random.Range(5, 10);
                print("Fini");
            }
        }

        transform.position = new Vector3(position_x, position_y, position_z);
        m_enemy.transform.position = transform.position;
        Instantiate(m_enemy);
    }
}
