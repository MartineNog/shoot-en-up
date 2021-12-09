using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Entity
{
    [SerializeField] private Camera m_MainCamera;
    private int m_PV_boss = 30;
    private float m_Speed = 2;
    private Vector3 m_direction_deplacement = Vector3.left;

    [SerializeField] private float m_Cadence_Shot;
    [SerializeField] private BulletBoss m_Bullets_Boss;

    private int m_nb_Bullets = 0;
    private int m_nb_Max_Bullets = 0;
    private float m_time;
    private bool m_is_Shot = false;

    private void Awake()
    {
        m_MainCamera = Camera.main;
        WriteCurrentPV(m_PV_boss);
    }

    private void Start()
    {
        StartCoroutine(Shot());
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (m_MainCamera.WorldToScreenPoint(transform.position).x <= 0 )
        {
            m_direction_deplacement = Vector3.right;
            if (m_is_Shot)
                m_Speed = Random.Range(2.5f, 5.5f);
            else
                m_Speed = Random.Range(2.5f, 10.5f);
        }
        else if (m_MainCamera.WorldToScreenPoint(transform.position).x >= Screen.width)
        {
            m_direction_deplacement = Vector3.left;
            if (m_is_Shot)
                m_Speed = Random.Range(2.5f, 5.5f);
            else
                m_Speed = Random.Range(2.5f, 10.5f);
        }

        transform.position += m_direction_deplacement * Time.deltaTime * m_Speed;
    }

    IEnumerator Shot()
    {
        while (Application.isPlaying)
        {
            if (!m_is_Shot)
            {
                m_Speed = Random.Range(2.5f, 10.5f);
                m_nb_Bullets = 0;
                m_nb_Max_Bullets = Random.Range(30, 60);
                m_time = Random.Range(5f, 10f);
                yield return new WaitForSeconds(m_time);
                m_is_Shot = true;
            }
            else
            {
                m_Speed = Random.Range(2.5f, 5.5f);
                m_Cadence_Shot = Random.Range(0.1f, 0.5f);
                m_Bullets_Boss.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                Instantiate(m_Bullets_Boss);
                m_nb_Bullets++;
                yield return new WaitForSeconds(m_Cadence_Shot);
            }

            if (m_nb_Bullets > m_nb_Max_Bullets)
                m_is_Shot = false;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si le boss est touché par un projectile
        if (collision.gameObject.tag == "Bullet")
        {
            WriteCurrentPV(ReducePV(5));
            // Si le boss n'a plus de vie
            if (ReadCurrentPV() <= 0)
            {
                Player.player_S.m_score += 50;
                Destroy(this.gameObject);
                Player.player_S.Victoire();
            }
        }
    }
}
