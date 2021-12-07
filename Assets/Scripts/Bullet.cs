using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] float m_Bullet_Speed;
    [SerializeField] private float m_margin;

    private void Awake()
    {
        m_MainCamera = Camera.main; 
    }

    void Start()
    {
        
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        if (m_MainCamera.WorldToScreenPoint(transform.position).y < (Screen.height + m_margin))
        {
            transform.position += Vector3.up * Time.deltaTime * m_Bullet_Speed;
        }
        else if (m_MainCamera.WorldToScreenPoint(transform.position).y >= (Screen.height + m_margin))
        {
            Destroy(this.gameObject);
        }
    }
}
