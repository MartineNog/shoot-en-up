using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Boss : Entity
{
    public static Boss Boss_S;

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
    private int m_option = 0;
    public bool m_bulletPatern = false;

    // Variables pour le bullet pattern
    private int m_nb_projectiles_BP;
    private float m_Speed_projectiles_BP;
    [SerializeField] private GameObject m_Projectiles_BP;
    private Vector3 m_startPoint;
    private const float m_raduis = 1F;

    [SerializeField] private GameObject m_Boss_Particules;
    private bool m_mort = false;

    private void Awake()
    {
        Boss_S = this;  // Initialiser le Singleton
        m_MainCamera = Camera.main;
        WriteCurrentPV(m_PV_boss);
    }

    private void Start()
    {
        StartCoroutine(Shot());
    }

    void Update()
    {
        if (!m_mort)
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
        while (Application.isPlaying && !m_mort)
        {
            if (!m_is_Shot)     // Le boss ne fait que se déplacer
            {
                m_bulletPatern = false;
                m_Speed = Random.Range(2.5f, 10.5f);
                m_option = Random.Range(1, 6);
                m_nb_Bullets = 0;
                if (m_option <= 2)
                    m_nb_Max_Bullets = Random.Range(30, 60);
                else
                    m_nb_Max_Bullets = Random.Range(4, 8);
                
                m_time = Random.Range(5f, 10f);
                
                yield return new WaitForSeconds(m_time);
                m_is_Shot = true;
                
            }
            else if (m_option <=2)   // Le boss envoie des projectiles aléatoirement
            {
                m_bulletPatern = false;
                m_Speed = Random.Range(2.5f, 5.5f);
                m_Cadence_Shot = Random.Range(0.1f, 0.5f);
                
                m_Bullets_Boss.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, 0);
                Instantiate(m_Bullets_Boss);
                
                m_nb_Bullets++;
                yield return new WaitForSeconds(m_Cadence_Shot);
            }
            else    // Le boss envoie des projectiles sous forme de bullet pattern
            {
                
                m_Speed = Random.Range(1.5f, 3.5f);
                m_Cadence_Shot = Random.Range(0.7f, 2.7f);
                
                m_startPoint = new Vector3(transform.position.x, transform.position.y -1f, 0);
                m_nb_projectiles_BP = Random.Range(20, 40);
                m_Speed_projectiles_BP = Random.Range(1.5f, 4.5f);

                ShotProjectilesBP(m_nb_projectiles_BP);

                m_nb_Bullets++;
                //m_bulletPatern = true;
                yield return new WaitForSeconds(m_Cadence_Shot);
                m_bulletPatern = false;
            }
            
            if (m_nb_Bullets > m_nb_Max_Bullets)
                m_is_Shot = false;
            
        }
    }

    private void ShotProjectilesBP(int nbProjectiles)
    {
        float angleStep = 360f / nbProjectiles;
        float angle = 0f;

        for (int i = 0; i< nbProjectiles; i++)
        {
            // Direction des projectiles
            float directionX = m_startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * m_raduis;
            float directionY = m_startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * m_raduis;

            Vector3 vect = new Vector3(directionX, directionY, 0);
            Vector3 dir = (vect - m_startPoint).normalized * m_Speed_projectiles_BP;

            GameObject tmp = Instantiate(m_Projectiles_BP, m_startPoint, Quaternion.identity);
            tmp.GetComponent<Rigidbody>().velocity = new Vector3(directionX, 0, directionY);

            angle += angleStep;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si le boss est touché par un projectile
        if (collision.gameObject.tag == "Bullet")
        {
            WriteCurrentPV(ReducePV(5));
            transform.localScale = new Vector3(transform.localScale.x + 0.025f, transform.localScale.y + 0.025f, transform.localScale.z + 0.025f);
            // Si le boss n'a plus de vie
            if (ReadCurrentPV() <= 0)
            {
                m_mort = true;
                Player.player_S.m_score += 50;

                /*Instantiate(m_Boss_Particules, new Vector3(transform.position.x, transform.position.y - 5f, 0), Quaternion.identity);
                Destroy(this.gameObject, 2f);
                Player.player_S.Victoire();*/

                StartCoroutine(ExplosionBoss());
            }
        }
    }

    IEnumerator ExplosionBoss()
    {
        Instantiate(m_Boss_Particules, new Vector3(transform.position.x, transform.position.y - 5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
        Player.player_S.Victoire();
    }
}
