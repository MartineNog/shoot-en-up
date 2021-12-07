using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_enemy;
    [SerializeField] private float m_Time_Enemies = 3;

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

        while (Application.isPlaying)
        {
            m_Time_Enemies = 3;     //Random.Range(0.3f,3);
            float borne = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).x;
            float position_x = Random.Range(-borne, borne);    // A mettre en aléatoire
            float position_y = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).y;  // A mettre en haut de l'écran
            float position_z = m_player.transform.position.z;   // Profondeur du joueur

            transform.position = new Vector3(position_x, position_y, position_z);
            m_enemy.transform.position = transform.position;
            Instantiate(m_enemy);
            yield return new WaitForSeconds(m_Time_Enemies);
        }   
    }
}
