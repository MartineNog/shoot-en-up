using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;
    [SerializeField] private float m_margin;


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
            // Eviter la sortie d'�cran vers la gauche ( si on sort de l'�cran vers la gauche on arrive dans des valeurs n�gatives)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x > (0 + m_margin))
            {
                transform.position += Vector3.left * Time.deltaTime * m_HorizontalSpeed;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Eviter la sortie d'�cran vers la droite ( si on sort de l'�cran vers la droite on arrive dans des valeurs sup�rieur � la taille de l'�cran)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x < (Screen.width - m_margin))
            {
                transform.position += Vector3.right * Time.deltaTime * m_HorizontalSpeed;
            }
        }
    }
}
