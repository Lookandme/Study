using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player { get; private set; }
    public ObjectPool ObjectPool { get; private set; }
    public ParticleSystem EffectParticle;

    [SerializeField] private string playerTag = "Player";
    private HealthSystem playerHealthSystem;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        Instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;

        ObjectPool = GetComponent<ObjectPool>();
        EffectParticle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();
        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;
    }

    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }
    private void GameOver()
    {
        // �ش� UI ���ֱ�
       gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}