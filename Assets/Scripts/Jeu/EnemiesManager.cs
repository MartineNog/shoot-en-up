using System.Collections;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [Header("Caméra principale")]
    [SerializeField] private Camera m_MainCamera;

    [Header("Personnages")]
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_enemy;
    [SerializeField] private GameObject m_boss;
    [SerializeField] private int m_margin_boss;
    
    [Header("Vagues")]
    [SerializeField] private float m_Time_Enemies = 3;
    private bool vague = false;
    private int nVague;
    int compteurVague = 3;

    private void Awake()
    {
        // Initialisation de la caméra principale
        m_MainCamera = Camera.main;

        // Initialisation du nombre d'ennemies avant une vague
        m_Time_Enemies = Random.Range(3, 7);
    }

    void Start()
    {
        StartCoroutine(Enemies());
    }

    private void Update()
    {
        // Il faut faire apparaitre le boss
        if (Player.player_S.boss == 1)
        {
            Player.player_S.boss = 2;
            float position_x = 0;
            float position_y = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).y + m_margin_boss;  // A mettre en haut de l'écran
            float position_z = m_player.transform.position.z;   // Profondeur du joueur
            m_boss.transform.position = new Vector3(position_x, position_y, position_z);
            Instantiate(m_boss);
        }
    }

    IEnumerator Enemies()
    {
        int compteur = 0;

        // Tant que le jeu est en cours et que c'est au ennemies d'apparaitre
        while (Application.isPlaying && Player.player_S.m_nb_Enemies < Player.player_S.m_nb_Max_Enemies)
        {
            float borne = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).x;

            // S'il est tant de lancer une vague d'ennemies
            if (compteurVague <= 0 && !vague)
            {
                // Indiquer que la vague est lancé
                vague = true;

                // Définir aléatoirement quel style de vague va être lancé
                nVague = Mathf.RoundToInt(Random.Range(0.6f, 3.4f));

                compteur = 0;
            }

            // Si une vague est en cours
            if (vague)
            {
                // Définir un temps entre chaque apparition d'ennemies
                m_Time_Enemies = 0.3f;

                // Lancer la vague
                WaveMove(borne, compteur);

                compteur++;
            }
            // Sinon
            else
            {
                // Définir le temps entre chaque apparition d'ennemies
                m_Time_Enemies = 3;

                // Lancer l'apparition aléatoire d'ennemies
                RandomMove(borne);

                compteurVague--;
            }

            // Faire une pause entre 2 apparitions d'ennemies
            yield return new WaitForSeconds(m_Time_Enemies);
        }   
    }

    // Apparition aléatoire des ennemies
    private void RandomMove(float borne)
    {
        // Définir la position d'apparition 
        float position_x = Random.Range(-borne, borne);
        float position_y = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).y;  // A mettre en haut de l'écran
        float position_z = m_player.transform.position.z;   // Profondeur du joueur

        // Instantier l'ennemie à la place définie
        transform.position = new Vector3(position_x, position_y, position_z);
        m_enemy.transform.position = transform.position;
        Instantiate(m_enemy);
    }

    // Apparition d'ennemies par vague
    private void WaveMove(float borne, int compteur)
    {
        // Définir la position d'apparition
        float position_x = 0;
        float position_y = m_MainCamera.ScreenToWorldPoint(m_MainCamera.transform.position).y;  // A mettre en haut de l'écran
        float position_z = m_player.transform.position.z;   // Profondeur du joueur

        // Deplacement en vague
        if (nVague == 1)
        {
            // Calcul de la position en x en fonction des précédent ennemies
            position_x = (-borne) + compteur * 1.5f;
            if (position_x >= borne)
            {
                vague = false;
                compteurVague = (int)Random.Range(5, 10);
            }

            // Instantier l'ennemie
            transform.position = new Vector3(-position_x, position_y, position_z);
            m_enemy.transform.position = transform.position;
            Instantiate(m_enemy);
        }
        // Déplacement en ligne vers la droite
        else if (nVague == 2)
        {
            // Calcul de la position en x en fonction des précédents ennemies
            position_x = (-borne) + compteur * 2;
            if (position_x >= borne)
            {
                vague = false;
                compteurVague = (int)Random.Range(5, 10);
            }
        }
        // Déplacement en ligne vers la gauche
        else if (nVague == 3)
        {
            // Calcul de la position en x en fonction des précédents ennemies
            position_x = borne - compteur * 2;
            if (position_x <= -borne)
            {
                vague = false;
                compteurVague = (int)Random.Range(5, 10);
            }
        }

        // Instancier l'ennemies
        transform.position = new Vector3(position_x, position_y, position_z);
        m_enemy.transform.position = transform.position;
        Instantiate(m_enemy);
    }
}
