using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;
    [SerializeField] private float m_margin;
    [SerializeField] private float m_Cadence_Shot;
    [SerializeField] private GameObject m_Bullets;
    [SerializeField] private Stopwatch m_Stopwatch;


    private void Awake()
    {
        m_MainCamera = Camera.main;
        m_Stopwatch = new Stopwatch();
        m_Stopwatch.Start();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
    }

    void PlayerControl()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Eviter la sortie d'écran vers la gauche ( si on sort de l'écran vers la gauche on arrive dans des valeurs négatives)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x > (0 + m_margin))
            {
                transform.position += Vector3.left * Time.deltaTime * m_HorizontalSpeed;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Eviter la sortie d'écran vers la droite ( si on sort de l'écran vers la droite on arrive dans des valeurs supérieur à la taille de l'écran)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x < (Screen.width - m_margin))
            {
                transform.position += Vector3.right * Time.deltaTime * m_HorizontalSpeed;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (m_Stopwatch.Elapsed.Milliseconds >= m_Cadence_Shot)
            {
                m_Bullets.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                Instantiate(m_Bullets);
                m_Stopwatch.Restart(); 
            }
        }
    }
}
