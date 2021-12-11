using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Text m_Score;
    [SerializeField] private Slider m_Slider_PV;
    [SerializeField] private Player player;
    [SerializeField] private GameObject m_Pause;
    
    // Bonus
    [SerializeField] private GameObject m_BonusCadenceBullet;
    [SerializeField] private GameObject m_BonusSpeedBullet;
    [SerializeField] private GameObject m_BonusSpeedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_Score.text = "Score : " + player.m_score;

        m_Slider_PV.minValue = 0;
        m_Slider_PV.maxValue = player.ReadCurrentPV();
        m_Slider_PV.value = player.ReadCurrentPV();

        m_Pause.SetActive(false);

        m_BonusCadenceBullet.SetActive(false);
        m_BonusSpeedBullet.SetActive(false);
        m_BonusSpeedPlayer.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        player.UserInterfaceChange.AddListener(OnHPChange);
        player.UserInterfaceChange.AddListener(OnScoreChange);
        player.UserInterfaceChange.AddListener(OnBonus);
    }

    public void  OnScoreChange()
    {
        m_Score.text = "Score : " + player.m_score;
    }

    public void OnHPChange()
    {
        // On change la valeur des PV
        m_Slider_PV.value = player.ReadCurrentPV();
    }

    public void OnBonus()
    {
        m_BonusCadenceBullet.SetActive(player.m_BonusCadenceBullet);
        m_BonusSpeedBullet.SetActive(player.m_BonusSpeedBullet);
        m_BonusSpeedPlayer.SetActive(player.m_BonusSpeedPlayer);
    }
}
