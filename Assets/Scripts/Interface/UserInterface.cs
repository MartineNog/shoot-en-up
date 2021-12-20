using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [Header("Joueur")]
    [SerializeField] private Player player;

    [Header("Emement de l'interface utilisateur")]
    [SerializeField] private Text m_Score;
    [SerializeField] private Slider m_Slider_PV;
    [SerializeField] private GameObject m_Pause;
    
    [Header("Bonus")]
    [SerializeField] private GameObject m_BonusCadenceBullet;
    [SerializeField] private GameObject m_BonusSpeedBullet;
    [SerializeField] private GameObject m_BonusSpeedPlayer;

    void Start()
    {
        // Affichage du score
        m_Score.text = "Score : " + player.m_score;

        // Initialiser le slider de vie du joueur
        m_Slider_PV.minValue = 0;
        m_Slider_PV.maxValue = player.ReadCurrentPV();
        m_Slider_PV.value = player.ReadCurrentPV();

        // Desactiver l'ecran de pause
        m_Pause.SetActive(false);

        // Desactiver tous les bonus
        m_BonusCadenceBullet.SetActive(false);
        m_BonusSpeedBullet.SetActive(false);
        m_BonusSpeedPlayer.SetActive(false);
    }

    void Update()
    {
        // Récupérer les changements d'affichage qui doivent être effectués 
        player.UserInterfaceChange.AddListener(OnHPChange);
        player.UserInterfaceChange.AddListener(OnScoreChange);
        player.UserInterfaceChange.AddListener(OnBonus);
    }

    // Si le score a changé
    public void  OnScoreChange()
    {
        // Mise a jours du score
        m_Score.text = "Score : " + player.m_score;
    }

    // Si les points de vie du joueur change
    public void OnHPChange()
    {
        // On change la valeur du slider des PV du joueur
        m_Slider_PV.value = player.ReadCurrentPV();
    }

    // Si les bonus doivent être afficher
    public void OnBonus()
    {
        m_BonusCadenceBullet.SetActive(player.m_BonusCadenceBullet);
        m_BonusSpeedBullet.SetActive(player.m_BonusSpeedBullet);
        m_BonusSpeedPlayer.SetActive(player.m_BonusSpeedPlayer);
    }
}
