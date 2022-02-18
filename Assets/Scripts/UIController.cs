using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static event Action OnToggleAutoSpawnButtonPressed;
    public static event Action OnStartNextWaveButtonPressed;
    public static event Action OnToggleGameSpeedButtonPressed;

    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI roundsUI;

    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas gameWonUI;

    [SerializeField] Button toggleAutoSpawnButton;
    [SerializeField] Button startNextWaveButton;
    [SerializeField] Button toggleGameSpeedButton;

    [SerializeField] InfoPanel infoPanel;

    private void OnEnable()
    {
        // Subscribe to events
        GameManager.OnHealthUpdate += UpdateHealthUI;
        GameManager.OnCurrencyUpdate += UpdateCurrencyUI;
        GameManager.OnRoundUpdate += UpdateRoundsUI;

        GameManager.OnGameStart += Setup;
        GameManager.OnGameOver += ShowGameOverUI;
        GameManager.OnSwitchGameSpeed += SetToggleGameSpeedButtonState;

        Spawner.OnLastWaveCleared += ShowGameWonUI;
        Spawner.OnWaveSpawned += () => ToggleStartNextWaveButton(false);
        Spawner.OnWaveCleared += (wave) => ToggleStartNextWaveButton(true);
        Spawner.OnToggleAutoSpawn += SetToggleAutoSpawnButtonState;

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
        GameManager.OnSwitchGameSpeed -= SetToggleGameSpeedButtonState;

        Spawner.OnLastWaveCleared -= ShowGameWonUI;
        Spawner.OnWaveSpawned -= () => ToggleStartNextWaveButton(false);
        Spawner.OnWaveCleared -= (wave) => ToggleStartNextWaveButton(true);
        Spawner.OnToggleAutoSpawn -= SetToggleAutoSpawnButtonState;

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

    public void ToggleAutoPlay()
    {
        OnToggleAutoSpawnButtonPressed?.Invoke();
    }

    public void StartNextWave()
    {
        OnStartNextWaveButtonPressed?.Invoke();
    }

    public void ToggleGameSpeed()
    {
        OnToggleGameSpeedButtonPressed?.Invoke();
    }

    public void ToggleStartNextWaveButton(bool active)
    {
        startNextWaveButton.gameObject.SetActive(active);
        toggleGameSpeedButton.gameObject.SetActive(!active);
    }

    void SetToggleGameSpeedButtonState(int gameSpeed)
    {
        toggleGameSpeedButton.GetComponent<Image>().color = gameSpeed > 1 ? Color.white : Color.gray;
    }

    void SetToggleAutoSpawnButtonState(bool active)
    {
        toggleAutoSpawnButton.GetComponent<Image>().color = active ? Color.white : Color.gray;
    }
}
