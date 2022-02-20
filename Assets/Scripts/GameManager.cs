using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static event Action<int> OnHealthUpdate;
    public static event Action<int> OnRoundUpdate;
    public static event Action<int> OnCurrencyUpdate;
    public static event Action<int> OnSwitchGameSpeed;

    public static event Action OnGameStart;
    public static event Action OnGameOver;

    [SerializeField] int startingHealth = 10;
    private int _health;
    public int health
    {
        get => _health;
        private set
        {
            _health = value;
            OnHealthUpdate?.Invoke(_health);
        }
    }

    [SerializeField] int startingCurrency = 100;
    private int _purchaseCurrency;
    public int purchaseCurrency
    {
        get => _purchaseCurrency;
        private set
        {
            _purchaseCurrency = value;
            OnCurrencyUpdate?.Invoke(_purchaseCurrency);
        }
    }

    private int _currentRound = 1;
    public int currentRound
    {
        get => _currentRound;
        private set
        {
            _currentRound = value;
            OnRoundUpdate?.Invoke(_currentRound);
        }
    }

    bool gameOver = false;
    bool gameWon = false;
    int gameSpeed = 1;

    private void OnEnable()
    {
        // Subscribe to events
        MonsterController.OnMonsterDied += (MonsterController monster) => purchaseCurrency += monster.currencyToDrop;
        MonsterController.OnMonsterReachedFinalWaypoint += (MonsterController monster) => TakeDamage(monster.damage);

        Spawner.OnWaveCleared += WaveCleared;
        Spawner.OnLastWaveCleared += WinGame;

        BuildingManager.OnTowerPlaced += (TowerController tower) => purchaseCurrency -= tower.cost;
        //TowerController.OnUpgradeActivated += (Upgrade upgrade) => purchaseCurrency -= upgrade.cost;

        OnGameOver += LoseGame;

        UIController.OnToggleGameSpeedButtonPressed += SwitchGameSpeed;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        MonsterController.OnMonsterDied -= (MonsterController monster) => purchaseCurrency += monster.currencyToDrop;
        MonsterController.OnMonsterReachedFinalWaypoint -= (MonsterController monster) => TakeDamage(monster.damage);

        Spawner.OnWaveCleared -= WaveCleared;
        Spawner.OnLastWaveCleared -= WinGame;

        BuildingManager.OnTowerPlaced -= (TowerController tower) => purchaseCurrency -= tower.cost;
        //TowerController.OnUpgradeActivated -= (Upgrade upgrade) => purchaseCurrency -= upgrade.cost;

        OnGameOver -= LoseGame;

        UIController.OnToggleGameSpeedButtonPressed -= SwitchGameSpeed;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = gameSpeed;

        health = startingHealth;
        purchaseCurrency = startingCurrency;

        // Emit all the starting values
        OnGameStart?.Invoke();
        OnSwitchGameSpeed?.Invoke(gameSpeed);
        OnHealthUpdate?.Invoke(startingHealth);
        OnCurrencyUpdate?.Invoke(startingCurrency);
        OnRoundUpdate?.Invoke(currentRound);
    }

    private void Update()
    {
        if ((gameOver || gameWon) && Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void TakeDamage(int damage)
    {
        health = Mathf.Max(health -= damage, 0);

        if (health <= 0)
        {
            OnGameOver?.Invoke();
        }
    }

    void LoseGame()
    {
        gameOver = true;
        Time.timeScale = 0;
    }

    void WinGame()
    {
        gameWon = true;
        Time.timeScale = 0;
    }

    void SwitchGameSpeed()
    {
        gameSpeed = gameSpeed == 1 ? 2 : 1;
        Time.timeScale = gameSpeed;
        OnSwitchGameSpeed?.Invoke(gameSpeed);
    }

    void WaveCleared(int roundCleared)
    {
        purchaseCurrency += 100 + roundCleared;
        // TODO: Should not be keeping track of current round in GameManager and Spawner.
        // Either wave all this into the Spawner, or find a way to keep track of the actual round in the GameManager while having the spawner just do the actual Spawning
        // That would mean we have to keep track of the waves in the GameManager.
        currentRound = roundCleared + 1;
    }
}
