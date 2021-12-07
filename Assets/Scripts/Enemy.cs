using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] float m_Enemy_Speed;
    [SerializeField] private float m_margin;

    private void Awake()
    {
        WriteCurrentPV(5);
        m_MainCamera = Camera.main;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si l'ennemi a toucher le joueur il disparait
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
                Destroy(this.gameObject);
            }
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
