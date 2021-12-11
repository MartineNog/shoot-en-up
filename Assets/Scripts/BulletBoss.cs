using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    private float m_Bullet_Speed;
    private float dir;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    private void Start()
    {
        dir = Random.Range(-1f, 1f);
        m_Bullet_Speed = Random.Range(5, 10);
    }

    void Update()
    {
        if (Boss.Boss_S.m_bulletPatern == false)
            MoveBullet();
    }
    void MoveBullet()
    {
        if (m_MainCamera.WorldToScreenPoint(transform.position).y > -5)
        {
            transform.position += new Vector3(dir, -1 , 0) * Time.deltaTime * m_Bullet_Speed;
        }
        else if (m_MainCamera.WorldToScreenPoint(transform.position).y <= -5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // S'il touche le joueur
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        // s'il est touché par un projectile
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
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


}
