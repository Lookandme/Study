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

    [SerializeField] private int currentWaveIndex = 0; // 웨이브 단계
    private int currentSpawnCount = 0; // 현재 몬스터 숫자
    private int waveSpawnCount = 0;  // 웨이브 당 생성해야 할 몬스터 숫자
    private int waveSpawnPosCount = 0;  //?

    public float spawnInterval = .5f; // 생성 딜레이
    public List<GameObject> enemyPrefebs = new List<GameObject>(); // 생성해야 할 몬스터 종류를 담는 리스트

    [SerializeField] private Transform spawnPositionsRoot; // 몬스터 생성 위치 부모객체를 등록하고 생성된 몬스터는 자식객체로
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

                yield return StartCoroutine(SpawnEnemiesInWave()); // 코루틴 리턴으로 다음 코루틴 발생

                currentWaveIndex++;

                yield return null;
            }
        }
    }



    void ProcessWaveConditions()
    {
        // % 는 나머지 연산자
        // 나머지 값에 따라 조건문을 주어서, 주기성이 있는 대상에 활용

        // 20 스테이지마다 이벤트가 발생
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
    void SpawnEnemyAtPosition(int posIdx)       // 다시 살펴 볼것!!
    {
        int prefabIdx = Random.Range(0, enemyPrefebs.Count);
        GameObject enemy = Instantiate(enemyPrefebs[prefabIdx], spawnPositions[posIdx].position, Quaternion.identity);
        // 생성한 적에 OnEnemyDeath를 등록해요.
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
        currentSpawnCount++;
    }

    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }

    private void IncreaseWaveSpawnCount()
    {
        waveSpawnCount++;       // 왜 3단계마다 올라가는지  질문 필요 웨이브 하나당 3번에 몬스터 스폰이 있나?
    }

    private void CreateReward()
    {
        Debug.Log("CreateReward 호출");       // 22강에서 계속
    }

    private void IncreaseSpawnPositions()
    {
        waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPositions.Count ? waveSpawnPosCount : waveSpawnPosCount + 1;
        waveSpawnCount = 0;
    }

    private void RandomUpgrade()
    {
        Debug.Log("RandomUpgrade 호출");
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
        // 해당 UI 켜주기
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