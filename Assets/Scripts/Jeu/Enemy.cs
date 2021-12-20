using UnityEngine;

public class Enemy : Entity
{
    [Header("Caméra principale")]
    [SerializeField] private Camera m_MainCamera;

    [Header("Information ennemies")]
    [SerializeField] float m_Enemy_Speed;
    [SerializeField] private float m_margin;

    [Header("Bonus")]
    [SerializeField] private GameObject m_Bonus_SpeedPlayer;
    [SerializeField] private GameObject m_Bonus_CadenceBalle;
    [SerializeField] private GameObject m_Bonus_SpeedBalle;
    [SerializeField] private GameObject m_BonusShield;

    private int m_PV_enemies = 5;

    private void Awake()
    {
        // Initialiser leur point de vie
        WriteCurrentPV(m_PV_enemies);

        // Initialiser la caméra principale
        m_MainCamera = Camera.main;
    }

    // Lorsque l'ennemie rentre en collision avec autrui
    private void OnCollisionEnter(Collision collision)
    {
        // Si l'ennemi a touché le joueur il disparait
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject, 0.3f);
        }

        // Si l'ennemi est touché par un projectile
        if (collision.gameObject.tag == "Bullet")
        {
            // On réduit sa vie
            WriteCurrentPV(ReducePV(6));

            // Si l'ennemie est n'a plus de vie 
            if (ReadCurrentPV() <= 0)
            {
                // Calculer un nombre aléatoire pour savoir s'il donne un bonus ou non
                int rand = Random.Range(0, 80);  
                
                // Instancier le bonus augmentant la vitesse du joueur
                if (rand == 16)  
                {
                    m_Bonus_SpeedPlayer.transform.position = transform.position;
                    Instantiate(m_Bonus_SpeedPlayer);
                }
                // Instancier le bonus du bouclier du joueur
                else if (rand == 32)  
                {
                    m_BonusShield.transform.position = transform.position;
                    Instantiate(m_BonusShield);
                }
                // Instancier le bonus augmentant la cadence de tir
                else if (rand == 48) 
                {
                    m_Bonus_CadenceBalle.transform.position = transform.position;
                    Instantiate(m_Bonus_CadenceBalle);
                }
                // Instancier le bonus augmentant la force de tir
                else if (rand == 64)
                {
                    m_Bonus_SpeedBalle.transform.position = transform.position;
                    Instantiate(m_Bonus_SpeedBalle);
                }

                // On le détruit
                Destroy(this.gameObject);
            }
        }
    }

    // Lorsque l'ennemie rentre en collision avec un objet
    private void OnTriggerEnter(Collider other)
    {
        // Si le projectile touche le bouclier
        if (other.gameObject.tag == "Shield")
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        // On fait descendre l'ennemie verticalement
        if (m_MainCamera.WorldToScreenPoint(transform.position).y > (0 - m_margin))
        {
            transform.position -= Vector3.up * Time.deltaTime * m_Enemy_Speed;
        }
        // Si l'ennemie n'a pas été détruit est quitte l'écran
        else if (m_MainCamera.WorldToScreenPoint(transform.position).y <= (0 - m_margin))
        {
            // On diminue le score et on met à jour l'interface utilisateur
            Player.player_S.m_score--;
            Player.player_S.UserInterfaceChange?.Invoke();

            // On détruit l'ennemie
            Destroy(this.gameObject);
        }
    }
}
