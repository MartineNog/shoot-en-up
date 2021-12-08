using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;

public class Player : Entity
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;
    [SerializeField] private float m_margin;
    [SerializeField] private float m_Cadence_Shot;
    [SerializeField] private Bullet m_Bullets;
    [SerializeField] private Stopwatch m_Stopwatch;
    public int m_score = 0;
    public UnityEvent UserInterfaceChange = new UnityEvent();

    private void Awake()
    {
        WriteCurrentPV(10);
        m_MainCamera = Camera.main;
        m_Stopwatch = new Stopwatch();
        m_Stopwatch.Start();
    }
   
    void Update()
    {
        PlayerControl(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si le joueur est touché par un ennemis, on réduit sa vie
        if (collision.gameObject.tag == "Enemy")
        {
            WriteCurrentPV(ReducePV(1));
            UserInterfaceChange?.Invoke();
            print($"Player = {ReadCurrentPV()}");
            // Si le joueur n'a plus de vie, on arrête la partie
            if (ReadCurrentPV() <= 0)
            {
                Destroy(this.gameObject);
                print("PERDU!!");
            }
        }
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
                Bullet bullet = Instantiate(m_Bullets);
                bullet.OnHit.AddListener(OnBulletHit);
                m_Stopwatch.Restart();
            }
        }
    }

    public void OnBulletHit()
    {
        m_score++;
        UserInterfaceChange?.Invoke();
    }

}
