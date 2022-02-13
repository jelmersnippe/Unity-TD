using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public TextMeshProUGUI healthUI;
    [SerializeField] int startingHealth = 10;
    private int _health;
    public int health
    {
        get => _health;
        set
        {
            _health = value;
            healthUI.text = "Health: " + _health.ToString();
        }
    }

    [SerializeField] public TextMeshProUGUI currencyUI;
    [SerializeField] int startingCurrency = 100;
    private int _purchaseCurrency;
    public int purchaseCurrency
    {
        get => _purchaseCurrency;
        set
        {
            _purchaseCurrency = value;
            currencyUI.text = "Currency: " + _purchaseCurrency.ToString();
        }
    }

    [SerializeField] public TextMeshProUGUI roundsUI;
    private int _currentRound;
    public int currentRound
    {
        get => _currentRound;
        set
        {
            _currentRound = value;
            roundsUI.text = "Round: " + _currentRound.ToString();
        }
    }

    bool gameOver = false;
    bool gameWon = false;

    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas gameWonUI;
    int gameSpeed = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Time.timeScale = gameSpeed;
    }

    private void Start()
    {
        health = startingHealth;
        purchaseCurrency = startingCurrency;
    }

    private void Update()
    {
        if (gameOver || gameWon)
        {
            if (Input.anyKey)
            {
                gameOverUI.gameObject.SetActive(false);
                gameWonUI.gameObject.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }
    }

    public void takeDamage(int damage)
    {
        health = Mathf.Max(health -= damage, 0);
        healthUI.text = "Health: " + health.ToString();

        if (health <= 0)
        {
            LoseGame();
        }
    }

    public void LoseGame()
    {
        gameOver = true;
        gameOverUI.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        gameWon = true;
        gameWonUI.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void SwitchGameSpeed()
    {
        gameSpeed = gameSpeed == 1 ? 2 : 1;
        Time.timeScale = gameSpeed;
    }
}
