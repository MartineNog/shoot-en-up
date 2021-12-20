using System.Collections;
using UnityEngine;

public class Boss : Entity
{
    public static Boss Boss_S;  // Avoir le boss accessible grâce au Singleton 

    [Header("Caméra principale")]
    [SerializeField] private Camera m_MainCamera;

    // Information du boss
    private int m_PV_boss = 50;
    private float m_Speed = 2;
    private Vector3 m_direction_deplacement = Vector3.left;

    [Header("Projectiles")]
    [SerializeField] private float m_Cadence_Shot;
    [SerializeField] private BulletBoss m_Bullets_Boss;

    // Information sur la forme des projectiles que le boss va envoyer
    private int m_nb_Bullets = 0;
    private int m_nb_Max_Bullets = 0;
    private float m_time;
    private bool m_is_Shot = false;
    private int m_option = 0;
    public bool m_bulletPatern = false;

    [Header("Bullet pattern")]
    private int m_nb_projectiles_BP;
    private float m_Speed_projectiles_BP;
    [SerializeField] private GameObject m_Projectiles_BP;
    private Vector3 m_startPoint;
    private const float m_raduis = 1F;

    [Header("Particules")]
    [SerializeField] private GameObject m_Boss_Particules;
    private bool m_mort = false;

    private void Awake()
    {
        // Initialisation du Singleton
        Boss_S = this;

        // Initialisation de la caméra principale
        m_MainCamera = Camera.main;

        // Initialisation des points de vie du boss
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

    // Déplacement du boss
    private void Move()
    {
        // Déplacement vers la droite
        if (m_MainCamera.WorldToScreenPoint(transform.position).x <= 0 )
        {
            m_direction_deplacement = Vector3.right;

            // Déterminer aléatoirement la vitesse de déplacement
            if (m_is_Shot)
                m_Speed = Random.Range(2.5f, 5.5f);
            else
                m_Speed = Random.Range(2.5f, 10.5f);
        }
        // Déplacement vers la gauche
        else if (m_MainCamera.WorldToScreenPoint(transform.position).x >= Screen.width)
        {
            m_direction_deplacement = Vector3.left;

            // Déterminer aléatoirement la vitesse de déplacement 
            if (m_is_Shot)
                m_Speed = Random.Range(2.5f, 5.5f);
            else
                m_Speed = Random.Range(2.5f, 10.5f);
        }
        // Déplacer le boss
        transform.position += m_direction_deplacement * Time.deltaTime * m_Speed;
    }

    // Les actions du boss (s'il fait que se déplacer ou s'il lance des projectiles)
    IEnumerator Shot()
    {
        while (Application.isPlaying && !m_mort)
        {
            // Si le boss de fait que se déplacer
            if (!m_is_Shot)
            {
                // Indiquer qu'il ne faut pas lancer de projectiles
                m_bulletPatern = false;
                m_nb_Bullets = 0;

                // Déterminer aléatoirement la vitesse du boss et comment ils lancera ces projectiles 
                m_Speed = Random.Range(2.5f, 10.5f);
                m_option = Random.Range(1, 6);
                
                // Déterminer le nombre de particules qu'il lancera en fonction de l'option d'envoie déterminé
                if (m_option <= 2)
                    m_nb_Max_Bullets = Random.Range(30, 60);
                else
                    m_nb_Max_Bullets = Random.Range(4, 8);
                
                // Déterminer le temps que le boss ne fait que se déplacer
                m_time = Random.Range(5f, 10f);
                
                // Mettre en pause la fonction le temps que le boss puisse se déplacer
                yield return new WaitForSeconds(m_time);

                // Indiquer que le boss est desormais pret à tirer
                m_is_Shot = true;
                
            }
            // Si le boss ebvoie des projectiles aléatoirement
            else if (m_option <=2)
            {
                // Mettre à jours le nombre de particules lancées
                m_bulletPatern = false;

                // Déterminer aléatoirement la vitesse du boss et sa cadence de tir
                m_Speed = Random.Range(2.5f, 5.5f);
                m_Cadence_Shot = Random.Range(0.1f, 0.5f);
                
                // Lancer les projectiles
                m_Bullets_Boss.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, 0);
                Instantiate(m_Bullets_Boss);
                
                // Augmenter le nombre de particules lancées 
                m_nb_Bullets++;

                // Mettre en pause la fonction en fonction de la cadence entre 2 tirs
                yield return new WaitForSeconds(m_Cadence_Shot);
            }
            // Si le boss envoie des projectiles sous forme de bullet pattern
            else
            {
                // Déterminer aléatoirement la vitesse du boss et sa cadence de tir
                m_Speed = Random.Range(1.5f, 3.5f);
                m_Cadence_Shot = Random.Range(0.7f, 2.7f);
                
                // Déterminer le nombre et la vitesse des particules le temps d'une projections de projectiles
                m_nb_projectiles_BP = Random.Range(20, 40);
                m_Speed_projectiles_BP = Random.Range(1.5f, 4.5f);
                
                // Lancer les projectiles en fonction d'un point de départ
                m_startPoint = new Vector3(transform.position.x, transform.position.y -1f, 0);
                ShotProjectilesBP(m_nb_projectiles_BP);

                // Augmenter le nombre de particules lancés
                m_nb_Bullets++;
                
                // Mettre en pause la fonction en fonction de la cadence entre 2 lancement de projectiles
                yield return new WaitForSeconds(m_Cadence_Shot);

                // Indiquer que le design pattern est terminé
                m_bulletPatern = false;
            }
            
            // Si le boss n'a plus de projectiles à lancer, il reprend son déplacement
            if (m_nb_Bullets > m_nb_Max_Bullets)
                m_is_Shot = false;
            
        }
    }

    // Déplacement des projectiles en bullet pattern
    private void ShotProjectilesBP(int nbProjectiles)
    {
        // Déterminer l'angle pour lequel le projectil devra être lancé
        float angleStep = 360f / nbProjectiles;
        float angle = 0f;

        for (int i = 0; i< nbProjectiles; i++)
        {
            // Calculer la direction des projectiles
            float directionX = m_startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * m_raduis;
            float directionY = m_startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * m_raduis;

            Vector3 vect = new Vector3(directionX, directionY, 0);
            Vector3 dir = (vect - m_startPoint).normalized * m_Speed_projectiles_BP;

            // Instantier les projectiles en fonction de l'angle et de la direction
            GameObject tmp = Instantiate(m_Projectiles_BP, m_startPoint, Quaternion.identity);
            tmp.GetComponent<Rigidbody>().velocity = new Vector3(directionX, 0, directionY);

            // Déplacer l'angle
            angle += angleStep;
        }
    }

    // Lorsque le boss rentre en collision avec autre chose
    private void OnCollisionEnter(Collision collision)
    {
        // Si le boss est touché par un projectile
        if (collision.gameObject.tag == "Bullet")
        {
            // On réduit sa vie
            WriteCurrentPV(ReducePV(5));

            // On le fait grossir 
            transform.localScale = new Vector3(transform.localScale.x + 0.025f, transform.localScale.y + 0.025f, transform.localScale.z + 0.025f);

            // Si le boss n'a plus de vie
            if (ReadCurrentPV() <= 0)
            {
                // On indique qu'il doit mourir 
                m_mort = true;

                // On augmente le score du joueur
                Player.player_S.m_score += 50;

                // On démarre l'explosion du boss
                StartCoroutine(ExplosionBoss());
            }
        }
    }

    // Explosion du boss
    IEnumerator ExplosionBoss()
    {
        // Instancier le system de particules 
        Instantiate(m_Boss_Particules, new Vector3(transform.position.x, transform.position.y - 5f, 0), Quaternion.identity);

        // Mettre en pause la fonction pour laisser le temps de voir les particules
        yield return new WaitForSeconds(1f);

        // Détruire le boss
        Destroy(this.gameObject);

        // Indiquer que le joueur a gagné
        Player.player_S.Victoire();
    }
}
