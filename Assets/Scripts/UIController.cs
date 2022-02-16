using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI roundsUI;

    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas gameWonUI;

    private void OnEnable()
    {
        // Subscribe to events
        GameManager.OnHealthUpdate += UpdateHealthUI;
        GameManager.OnCurrencyUpdate += UpdateCurrencyUI;
        GameManager.OnRoundUpdate += UpdateRoundsUI;

        GameManager.OnGameStart += Setup;
        GameManager.OnGameOver += ShowGameOverUI;
        Spawner.OnLastWaveCleared += ShowGameWonUI;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        GameManager.OnHealthUpdate -= UpdateHealthUI;
        GameManager.OnCurrencyUpdate -= UpdateCurrencyUI;
        GameManager.OnRoundUpdate -= UpdateRoundsUI;

        GameManager.OnGameStart -= Setup;
        GameManager.OnGameOver -= ShowGameOverUI;
        Spawner.OnLastWaveCleared -= ShowGameWonUI;
    }

    void Setup()
    {
        gameOverUI.gameObject.SetActive(false);
        gameWonUI.gameObject.SetActive(false);
    }

    void UpdateHealthUI(int health)
    {
        healthUI.text = "Health: " + health.ToString();
    }

    void UpdateCurrencyUI(int currency)
    {
        currencyUI.text = "Currency: " + currency.ToString();
    }

    void UpdateRoundsUI(int round)
    {
        roundsUI.text = "Round: " + round.ToString();
    }

    void ShowGameOverUI()
    {
        gameOverUI.gameObject.SetActive(true);
    }

    void ShowGameWonUI()
    {
        gameWonUI.gameObject.SetActive(true);
    }
}
