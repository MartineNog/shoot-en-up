using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    [Header("Cam�ra principale")]
    [SerializeField] private Camera m_MainCamera;

    [Header("Information projectile")]
    [SerializeField] public float m_Bullet_Speed;
    [SerializeField] private float m_margin;

    [Header("Evenement ")]
    public UnityEvent OnHit = new UnityEvent();

    private void Awake()
    {
        // Initialisation de la cam�ra principale
        m_MainCamera = Camera.main;
    }

    void Update()
    {
        MoveBullet();
    }

    // D�placement du projectile
    void MoveBullet()
    {
        // Si le projectile est dans le champs de vision on le fait d�placer vers le bas
        if (m_MainCamera.WorldToScreenPoint(transform.position).y < (Screen.height + m_margin))
            transform.position += Vector3.up * Time.deltaTime * m_Bullet_Speed;
        // Si le projectile est en dehors du champs de vision, on le d�truit
        else if (m_MainCamera.WorldToScreenPoint(transform.position).y >= (Screen.height + m_margin))
            Destroy(this.gameObject);
    }
  
    // Lorsque le projectile touche autrui
    private void OnCollisionEnter(Collision collision)
    {
        // Si le projectile a toucher un ennemi
        if (collision.gameObject.tag == "Enemy")
        {
            // On augmente le score du joueur et on met � jour l'interface utilisateur
            Player.player_S.m_score+=2;
            OnHit?.Invoke();

            // On averti de l'effet sonore qu'il faut jouer
            Player.player_S.EffetSonore = 2;
            
            // On d�truit le projectile
            Destroy(this.gameObject);
        }

        // Si le projectile a touch� le boss
        if (collision.gameObject.tag == "Boss")
        {
            // On augmente le score du joueur et on met � jour l'interface utilisateur
            Player.player_S.m_score += 5;
            OnHit?.Invoke();

            // On averti de l'effet sonore qu'il faut jouer
            Player.player_S.EffetSonore = 2;

            // On d�truit le projectile
            Destroy(this.gameObject);
        }

        // Si le projectile a touch� un projectile du boss
        if (collision.gameObject.tag == "BossBullet")
        {
            // On averti de l'effet sonore qu'il faut jouer
            Player.player_S.EffetSonore = 3;

            // On d�truit le projectile
            Destroy(this.gameObject);
        }
    }
}
