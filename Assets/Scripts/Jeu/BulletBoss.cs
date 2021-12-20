using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    [Header("Caméra principale")]
    [SerializeField] private Camera m_MainCamera;

    private float m_Bullet_Speed;
    private float dir;

    private void Awake()
    {
        // Initialisation de la caméra principale
        m_MainCamera = Camera.main;
    }

    private void Start()
    {
        // Définir aléatoirement une direction et une vitesse des projectiles du boss
        dir = Random.Range(-1f, 1f);
        m_Bullet_Speed = Random.Range(5, 10);
    }

    void Update()
    {
        if (Boss.Boss_S.m_bulletPatern == false)
            MoveBullet();
    }

    // Déplacement du projectile
    void MoveBullet()
    {
        if (m_MainCamera.WorldToScreenPoint(transform.position).y > -5)
        {
            transform.position += new Vector3(dir, -1 , 0) * Time.deltaTime * m_Bullet_Speed;
        }
        // Si le projectile est hors du champs de vision, on le détruit
        else if (m_MainCamera.WorldToScreenPoint(transform.position).y <= -5)
        {
            Destroy(this.gameObject);
        }
    }

    // Si le projectile rentre en collision avec autruit, on le détruit
    private void OnCollisionEnter(Collision collision)
    {
        // S'il touche le joueur
        if (collision.gameObject.tag == "Player")
            Destroy(this.gameObject);

        // s'il est touché par un projectile
        if (collision.gameObject.tag == "Bullet")
            Destroy(this.gameObject);
    }
   
    // Si le projectile rentre en collisiton avec un objet, on le détruit
    private void OnTriggerEnter(Collider other)
    {
        // Si le projectile touche le bouclier
        if (other.gameObject.tag == "Shield")
            Destroy(this.gameObject);
    }
}
