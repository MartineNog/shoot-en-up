using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] float m_Enemy_Speed;
    [SerializeField] private float m_margin;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
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
