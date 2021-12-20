using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine.Events;

public class Player : Entity
{
    public static Player player_S;  // Pour faire un Singleton

    [Header("Camera principale")]
    [SerializeField] private Camera m_MainCamera;

    [Header("Deplacement joueur")]
    [SerializeField] private float m_PlayerSpeed;
    [SerializeField] private float m_Margin_Horizontale;
    [SerializeField] private float m_Margin_Verticale;

    [Header("Tir du joueur")]
    [SerializeField] private float m_Cadence_Shot;
    [SerializeField] private Bullet m_Bullets;
    [SerializeField] private Stopwatch m_Stopwatch;
    public float m_BulletSpeed = 3;

    [Header("Apparition Boss")]
    [SerializeField] public int m_nb_Max_Enemies; 
    public int m_nb_Enemies = 0;
    public int boss = 0;

    [Header("Bonus")]
    [SerializeField] private GameObject m_Shield;
    public bool m_BonusCadenceBullet = false;
    public bool m_BonusSpeedBullet = false;
    public bool m_BonusSpeedPlayer = false;
    public bool m_BonusShield = false;
    public int MusiqueAvant = 0;

    [Header("Son")]
    public int Musique = 1;
    public int EffetSonore = 0;
    public bool Changement = false;

    [Header("Interface utilisateur")]
    public int m_score = 0;
    public UnityEvent UserInterfaceChange = new UnityEvent();

    private void Awake()
    {
        // Initialiser le Singleton et le joueur
        player_S = this;
        WriteCurrentPV(30);

        // Récupérer la caméra principale
        m_MainCamera = Camera.main;

        // Pouvoir récupérer le temps qui défile 
        m_Stopwatch = new Stopwatch();
        m_Stopwatch.Start();

        // Désactiver les bonus
        m_Shield.SetActive(false);

        // Cgoisir un nombre d'ennemies à toucher avant que le boss apparaisse
        m_nb_Max_Enemies = Random.Range(40, 60);
    }
   
    void Update()
    {
        PlayerControl();
    }

    // Interactions avec l'utilisateur (déplacement plus tirs)
    void PlayerControl()
    {
        // Déplacement vers la gauche du personnage
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Eviter la sortie d'écran vers la gauche ( si on sort de l'écran vers la gauche on arrive dans des valeurs négatives)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x > (0 + m_Margin_Horizontale))
                transform.position += Vector3.left * Time.deltaTime * m_PlayerSpeed;
        }

        // Déplacement vers la droite du personnage
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Eviter la sortie d'écran vers la droite ( si on sort de l'écran vers la droite on arrive dans des valeurs supérieur à la taille de l'écran)
            if (m_MainCamera.WorldToScreenPoint(transform.position).x < (Screen.width - m_Margin_Horizontale))
                transform.position += Vector3.right * Time.deltaTime * m_PlayerSpeed;
        }

        // Déplacement vers le haut du personnage
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Eviter la sortie d'écran vers le haut
            if (m_MainCamera.WorldToScreenPoint(transform.position).y <(Screen.height - m_Margin_Verticale))
                transform.position += Vector3.up * Time.deltaTime * m_PlayerSpeed;
        }

        // Déplacement vers le bas du personnage
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Eviter la sortie d'écran vers le bas
            if (m_MainCamera.WorldToScreenPoint(transform.position).y > (0 + m_Margin_Verticale))
                transform.position += Vector3.down * Time.deltaTime * m_PlayerSpeed;
        }

        // Le personnage tire à l'appuie sur la touche espace
        if (Input.GetKey(KeyCode.Space))
        {
            // On controle la cadence de tir
            if (m_Stopwatch.Elapsed.Milliseconds >= m_Cadence_Shot)
            {
                // On initialise la position et la vitesse du projectile
                m_Bullets.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                m_Bullets.m_Bullet_Speed = m_BulletSpeed;

                // On l'instantie
                Bullet bullet = Instantiate(m_Bullets);

                // On récupère les actions associées au tirs d'une balle
                bullet.OnHit.AddListener(OnBulletHit);

                // On réinitialise le compteur de cadence de tir 
                m_Stopwatch.Restart();
            }
        }
    }

    // Lorsque le joueur rentre en collision avec un ennemie
    private void OnCollisionEnter(Collision collision)
    {
        // Si le joueur est touché par un ennemis
        if (collision.gameObject.tag == "Enemy")
        {
            // On réduit sa vie
            WriteCurrentPV(ReducePV(1));

            // On active le bon effet sonore associé
            EffetSonore = 1;

            // On actualise l'interface utilisateur
            UserInterfaceChange?.Invoke();

            // On controle la vie du joueur
            PlayerIsDead();
        }

        // Si le joueur est touché par le boss
        if (collision.gameObject.tag == "Boss")
        {
            // On réduit sa vie
            WriteCurrentPV(ReducePV(3));

            // On active le bon effet sonore associé
            EffetSonore = 1;

            // On actualise l'interface utilisateur
            UserInterfaceChange?.Invoke();

            // On controle la vie du joueur
            PlayerIsDead();
        }

        // Si le joueur est touché par un projectile du boss
        if (collision.gameObject.tag == "BossBullet")
        {
            // On réduit sa vie
            WriteCurrentPV(ReducePV(1));

            // On active le bon effet sonore associé
            EffetSonore = 1;

            // On actualise l'interface utilisateur
            UserInterfaceChange?.Invoke();

            // On controle la vie du joueur
            PlayerIsDead();
        }
    }

    // Lorsque le joueur rentre en collision avec un bonus à récupérer
    private void OnTriggerEnter(Collider other)
    {
        // Si le joueur récupère le bonus d'augmentation de vitesse du joueur
        if (other.gameObject.tag == "BonusSpeedPlayer")
        {
            // On vérifie que le bonus n'est pas déjà activé
            if (!m_BonusSpeedPlayer)
            {
                // On active le bon effet sonre associé
                EffetSonore = 4;

                // On détruit le bonus
                Destroy(other.gameObject);

                // On exécute les actions associées à ce bonus
                StartCoroutine(BonusSpeedPlayer());
            }
        }

        // Si le joueur récupère le bonus du bouclier
        if (other.gameObject.tag == "BonusShield")
        {
            // On vérifie que le bonus n'est pas déjà activé
            if (!m_BonusShield)
            {
                // On détruit le bonus
                Destroy(other.gameObject);

                // On exécute les actions associées à ce bonus
                StartCoroutine(BonusShield());
            } 
        }

        // Si le joueur récupère le bonus d'augmentation de la cadence de tir
        if (other.gameObject.tag == "BonusBullet")
        {
            // On vérifie que le bonus n'est pas déjà activé
            if (!m_BonusCadenceBullet)
            {
                // On active le bon effet sonre associé
                EffetSonore = 4;

                // On détruit le bonus
                Destroy(other.gameObject);

                // On exécute les actions associées à ce bonus
                StartCoroutine(BonusBullet());
            }
        }

        // Si le joueur récupère le bonus d'augmentation de la vitesse des balles
        if (other.gameObject.tag == "BonusPowerBullet")
        {
            // On vérifie que le bonus n'est pas déjà activé
            if (!m_BonusSpeedBullet)
            {
                // On active le bon effet sonre associé
                EffetSonore = 4;

                // On détruit le bonus
                Destroy(other.gameObject);

                // On exécute les actions associées à ce bonus
                StartCoroutine(BonusPowerBullet());
            }
        }
    }

    // Regarder si le joueur est toujours vivant ou non
    void PlayerIsDead()
    {
        // Si le joueur n'a plus de vie, on arrête la partie
        if (ReadCurrentPV() <= 0)
        {
            StartCoroutine(FinPartie());
        }
    }

    // Lorsqu'un ennemie a été touché
    public void OnBulletHit()
    {
        // On augmente le nombre d'ennemies éliminés
        m_nb_Enemies++;

        // On actualise l'interface utilisateur
        UserInterfaceChange?.Invoke();

        // Si le nombre d'ennemies à éliminer a atteint le maximum
        if (m_nb_Enemies >= m_nb_Max_Enemies && boss == 0)
        {
            // On indique que c'est la musique du boss qui doit être jouée
            Musique = 2;
            Changement = true;

            // On indique que le boss doit être instantié et que les ennemies ne doivent plus apparaitre
            boss = 1;
        }
    }

    // Lorsque la partie est gagnée
    public void Victoire()
    {
        // On enregistre l'état et le score de la partie
        PlayerPrefs.SetInt("Fin", 1);
        PlayerPrefs.SetInt("Score", m_score);

        // On charge la page de fin de partie
        SceneManager.LoadScene(2);
    }

    // Lorsque la partie est perdue
    IEnumerator FinPartie()
    {
        yield return new WaitForSeconds(1f);

        // On sauvegarde l'état et le score de la partie
        PlayerPrefs.SetInt("Fin", -1);
        PlayerPrefs.SetInt("Score", m_score);

        // On détruit le joueur
        Destroy(this.gameObject);

        // On charge la page de fin de partie
        SceneManager.LoadScene(2);
    }

    // Lorsque le joueur a récupéré le bonus d'augmentation de vitesse du personnage
    IEnumerator BonusSpeedPlayer()
    {
        // On détermine aléatoirement le temps que le bonus sera effectif
        float time = Random.Range(10f, 20f);

        // On récupère la vitesse initiale du joueur
        float playerSpeed = m_PlayerSpeed;

        // On augmente la vitesse du joueur
        m_PlayerSpeed += 3;

        // On indique que le bonus est activé et on met à jour l'interface utilisateur
        m_BonusSpeedPlayer = true;
        UserInterfaceChange?.Invoke();

        // On met en pause la fonction le temps que le bonus sera effectif
        yield return new WaitForSeconds(time);

        // On remet la vitesse initiale du joueur
        m_PlayerSpeed = playerSpeed;

        // On indique que le bonus est terminé et on met à jour l'interface utilisateur
        m_BonusSpeedPlayer = false;
        UserInterfaceChange?.Invoke();
    }

    // Lorsque le joueur a récupéré le bonus d'augmentation de vitesse des projectiles
    IEnumerator BonusPowerBullet()
    {
        // On détermine aléatoirement le temps que le bonus sera effectif
        float time = Random.Range(10f, 20f);

        // On augmente la vitesse du projectile
        m_BulletSpeed = 6;

        // On indique que le bonus est activé et on met à jour l'interface utilisateur
        m_BonusSpeedBullet = true;
        UserInterfaceChange?.Invoke();

        // On met en pause la fonction le temps que le bonus sera effectif
        yield return new WaitForSeconds(time);

        // On remet la vitesse initiale du projectile
        m_BulletSpeed = 3;

        // On indique que le bonus est terminé et on met à jour l'interface utilisateur
        m_BonusSpeedBullet = false;
        UserInterfaceChange?.Invoke();
    }

    // Lorsque le joueur a récupéré le bonus d'augentation de la cadence des projetiles
    IEnumerator BonusBullet()
    {
        // On determine aléatoirement le temps que le bonus sera effectif
        float time = Random.Range(10f, 20f);

        // On récupère la cadence initiale de tir
        float cadenceShot = m_Cadence_Shot;

        // On diminue la cadence de tir 
        m_Cadence_Shot /= 2;

        // On indique que le bonus est activé et on met à jour l'interface utilisateur
        m_BonusCadenceBullet = true;
        UserInterfaceChange?.Invoke();

        // On met en pause la fonction le temps que le bonus sera effectif
        yield return new WaitForSeconds(time);

        // On remet la cadence initiale
        m_Cadence_Shot = cadenceShot;

        // On indique que le bonus est terminé et on met à jour l'interface utilisateur
        m_BonusCadenceBullet = false;
        UserInterfaceChange?.Invoke();
    }

    // Lorsque le joueur a récupéré le bonus du bouclier
    IEnumerator BonusShield()
    {
        // On determine aléatoirement le temps que le bonus sera effectif
        float time = Random.Range(10f, 20f);

        // On indique que le bonus est activé et on l'active
        m_BonusShield = true;
        m_Shield.SetActive(true);

        // On met change la musique du jeu
        MusiqueAvant = Musique;
        Musique = 3;
        Changement = true;
        
        // On met en pause la fonction le temps que le bonus sera effectif
        yield return new WaitForSeconds(time);

        // On indique que le bonus est terminé et on le desactif
        m_BonusShield = false;
        m_Shield.SetActive(false);

        // On remet la musique du jeu
        Musique = MusiqueAvant;
        Changement = true;
    }
}
