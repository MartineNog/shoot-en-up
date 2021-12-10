using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine.Events;

public class Player : Entity
{
    public static Player player_S;

    [SerializeField] private Camera m_MainCamera;

    // Option pour le déplacement
    [SerializeField] private float m_VerticalSpeed;
    [SerializeField] private float m_HorizontalSpeed;
    [SerializeField] private float m_Margin_Horizontale;
    [SerializeField] private float m_Margin_Verticale;

    // Options pour le tir du joueur
    [SerializeField] private float m_Cadence_Shot;
    [SerializeField] private Bullet m_Bullets;
    [SerializeField] private Stopwatch m_Stopwatch;

    // Option pour l'affichage du score et de la vie sur le canvas
    public int m_score = 0;
    public UnityEvent UserInterfaceChange = new UnityEvent();

    //Gestion d'apparition du boss
    [SerializeField] public int m_nb_Max_Enemies; 
    public int m_nb_Enemies = 0;
    public int boss = 0;

    private void Awake()
    {
        player_S = this;
        WriteCurrentPV(30);
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
            PlayerIsDead();
        }

        // Si le joueur est touché par le boss, on réduit sa vie
        if (collision.gameObject.tag == "Boss")
        {
            WriteCurrentPV(ReducePV(3));
            UserInterfaceChange?.Invoke();
            PlayerIsDead();
        }

        // Si le joueur est touché par un projectile du boss
        if (collision.gameObject.tag == "BossBullet")
        {
            WriteCurrentPV(ReducePV(1));
            UserInterfaceChange?.Invoke();
            PlayerIsDead();
        }
    }

    void PlayerIsDead()
    {
        // Si le joueur n'a plus de vie, on arrête la partie
        if (ReadCurrentPV() <= 0)
        {
            Destroy(this.gameObject);
            PlayerPrefs.SetInt("Fin", -1);  // On sauvegarde l'état de la partie
            PlayerPrefs.SetInt("Score", m_score);   // On sauvegarde le score
            SceneManager.LoadScene(2);  // On charge la page de fin de partie
        }
    }

    void PlayerControl()
    {
        // Déplacement vers la gauche du personnage
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Eviter la sortie d'écran vers la gauche ( si on sort de l'écran vers la gauche on arrive dans des valeurs négatives)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x > (0 + m_Margin_Horizontale))
            {
                transform.position += Vector3.left * Time.deltaTime * m_HorizontalSpeed;
            }
        }

        // Déplacement vers la droite du personnage
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Eviter la sortie d'écran vers la droite ( si on sort de l'écran vers la droite on arrive dans des valeurs supérieur à la taille de l'écran)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x < (Screen.width - m_Margin_Horizontale))
            {
                transform.position += Vector3.right * Time.deltaTime * m_HorizontalSpeed;
            }
        }

        // Déplacement vers le haut du personnage
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Eviter la sortie d'écran vers le haut
            if (m_MainCamera.WorldToScreenPoint(transform.position).y <(Screen.height - m_Margin_Verticale))
            {
                transform.position += Vector3.up * Time.deltaTime * m_VerticalSpeed;
            }
        }

        // Déplacement vers le bas du personnage
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Eviter la sortie d'écran vers le bas
            if (m_MainCamera.WorldToScreenPoint(transform.position).y > (0 + m_Margin_Verticale))
            {
                transform.position += Vector3.down * Time.deltaTime * m_VerticalSpeed;
            }
        }

        // Le personnage tire à l'appuie sur la touche espace
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
        m_nb_Enemies++;
        m_score++;
        UserInterfaceChange?.Invoke();

        if (m_nb_Enemies >= m_nb_Max_Enemies && boss == 0)
        {
            boss = 1;
            //Victoire();
        }

    }

    IEnumerator FinPartie()
    {
        yield return 0.1f;
        PlayerPrefs.SetInt("Fin", -1);  // On sauvegarde l'état de la partie
        PlayerPrefs.SetInt("Score", m_score);   // On sauvegarde le score
        SceneManager.LoadScene(2);
    }

    public void Victoire()
    {
        PlayerPrefs.SetInt("Fin", 1);  // On sauvegarde l'état de la partie
        PlayerPrefs.SetInt("Score", m_score);   // On sauvegarde le score
        SceneManager.LoadScene(2);
    }
}
