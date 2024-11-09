using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    [SerializeField] private int currentWaveIndex = 0; // ���̺� �ܰ�
    private int currentSpawnCount = 0; // ���� ���� ����
    private int waveSpawnCount = 0;  // ���̺� �� �����ؾ� �� ���� ����
    private int waveSpawnPosCount = 0;  //?

    public float spawnInterval = .5f; // ���� ������
    public List<GameObject> enemyPrefebs = new List<GameObject>(); // �����ؾ� �� ���� ������ ��� ����Ʈ

    [SerializeField] private Transform spawnPositionsRoot; // ���� ���� ��ġ �θ�ü�� ����ϰ� ������ ���ʹ� �ڽİ�ü��
    private List<Transform> spawnPositions = new List<Transform>();

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

        for (int i = 0; i < spawnPositionsRoot.childCount; i++) 
        {
            spawnPositions.Add(spawnPositionsRoot.GetChild(i));
        }
    }
    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        while (true) 
        {
            if(currentSpawnCount == 0)
            {
                UpdaetWaveUI();

                yield return new WaitForSeconds(2f);

                ProcessWaveConditions();

                yield return StartCoroutine(SpawnEnemiesInWave()); // �ڷ�ƾ �������� ���� �ڷ�ƾ �߻�

                currentWaveIndex++;

                yield return null;
            }
        }
    }



    void ProcessWaveConditions()
    {
        // % �� ������ ������
        // ������ ���� ���� ���ǹ��� �־, �ֱ⼺�� �ִ� ��� Ȱ��

        // 20 ������������ �̺�Ʈ�� �߻�
        if (currentWaveIndex % 20 == 0)
        {
            RandomUpgrade();
        }

        if (currentWaveIndex % 10 == 0)
        {
            IncreaseSpawnPositions();
        }

        if (currentWaveIndex % 5 == 0)
        {
            CreateReward();
        }

        if (currentWaveIndex % 3 == 0)
        {
            IncreaseWaveSpawnCount();       
        }
    }

    IEnumerator SpawnEnemiesInWave()
    {
        for (int i = 0; i < waveSpawnPosCount; i++)
        {
            int posIdx = Random.Range(0, spawnPositions.Count);
            for (int j = 0; j < waveSpawnCount; j++)
            {
                SpawnEnemyAtPosition(posIdx);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
    void SpawnEnemyAtPosition(int posIdx)       // �ٽ� ���� ����!!
    {
        int prefabIdx = Random.Range(0, enemyPrefebs.Count);
        GameObject enemy = Instantiate(enemyPrefebs[prefabIdx], spawnPositions[posIdx].position, Quaternion.identity);
        // ������ ���� OnEnemyDeath�� ����ؿ�.
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
        currentSpawnCount++;
    }

    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }

    private void IncreaseWaveSpawnCount()
    {
        waveSpawnCount++;       // �� 3�ܰ踶�� �ö󰡴���  ���� �ʿ� ���̺� �ϳ��� 3���� ���� ������ �ֳ�?
    }

    private void CreateReward()
    {
        Debug.Log("CreateReward ȣ��");       // 22������ ���
    }

    private void IncreaseSpawnPositions()
    {
        waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPositions.Count ? waveSpawnPosCount : waveSpawnPosCount + 1;
        waveSpawnCount = 0;
    }

    private void RandomUpgrade()
    {
        Debug.Log("RandomUpgrade ȣ��");
    }

    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }
    private void UpdaetWaveUI()
    {
        waveText.text = (currentWaveIndex + 1).ToString();
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