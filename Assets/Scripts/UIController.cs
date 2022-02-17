using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static event Action OnStartNextWaveButtonPressed;
    public static event Action OnToggleGameSpeedButtonPressed;

    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI roundsUI;

    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas gameWonUI;

    [SerializeField] Button startNextWaveButton;
    [SerializeField] Button switchGameSpeedButton;

    [SerializeField] InfoPanel infoPanel;

    private void OnEnable()
    {
        // Subscribe to events
        GameManager.OnHealthUpdate += UpdateHealthUI;
        GameManager.OnCurrencyUpdate += UpdateCurrencyUI;
        GameManager.OnRoundUpdate += UpdateRoundsUI;

        GameManager.OnGameStart += Setup;
        GameManager.OnGameOver += ShowGameOverUI;
        GameManager.OnSwitchGameSpeed += SetSwitchGameSpeedButtonState;

        Spawner.OnLastWaveCleared += ShowGameWonUI;
        Spawner.OnWaveSpawned += () => ToggleStartNextWaveButton(false);
        Spawner.OnWaveCleared += (wave) => ToggleStartNextWaveButton(true);

        BuildingManager.OnTowerPlaced += infoPanel.SetSelectedTower;
        BuildingManager.OnDeselectTower += infoPanel.DeselectSelectedTower;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        GameManager.OnHealthUpdate -= UpdateHealthUI;
        GameManager.OnCurrencyUpdate -= UpdateCurrencyUI;
        GameManager.OnRoundUpdate -= UpdateRoundsUI;

        GameManager.OnGameStart -= Setup;
        GameManager.OnGameOver -= ShowGameOverUI;
        GameManager.OnSwitchGameSpeed -= SetSwitchGameSpeedButtonState;

        Spawner.OnLastWaveCleared -= ShowGameWonUI;
        Spawner.OnWaveSpawned -= () => ToggleStartNextWaveButton(false);
        Spawner.OnWaveCleared -= (wave) => ToggleStartNextWaveButton(true);

        BuildingManager.OnTowerPlaced -= infoPanel.SetSelectedTower;
        BuildingManager.OnDeselectTower -= infoPanel.DeselectSelectedTower;
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

    public void StartNextWave()
    {
        OnStartNextWaveButtonPressed?.Invoke();
    }

    public void SwitchGameSpeed()
    {
        OnToggleGameSpeedButtonPressed?.Invoke();
    }

    void ToggleStartNextWaveButton(bool active)
    {
        startNextWaveButton.gameObject.SetActive(active);
        switchGameSpeedButton.gameObject.SetActive(!active);
    }

    void SetSwitchGameSpeedButtonState(int gameSpeed)
    {
        if (gameSpeed > 1)
        {
            switchGameSpeedButton.GetComponent<Image>().color = Color.white;
        } 
        else
        {
            switchGameSpeedButton.GetComponent<Image>().color = Color.gray;
        }
    }
}
