using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static event Action OnToggleAutoSpawnButtonPressed;
    public static event Action OnStartNextWaveButtonPressed;
    public static event Action OnToggleGameSpeedButtonPressed;
    public static event Action<float> OnVolumeChanged;

    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI roundsUI;

    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas gameWonUI;

    [SerializeField] Button toggleAutoSpawnButton;
    [SerializeField] Button startNextWaveButton;
    [SerializeField] Button toggleGameSpeedButton;

    [SerializeField] InfoPanel infoPanel;

    [SerializeField] Slider volumeControl;

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
        Spawner.OnToggleAutoSpawn += SetToggleAutoSpawnButtonState;
        Spawner.OnWaveCleared += (wave) => ToggleStartNextWaveButton(true);
        Spawner.OnWaveSpawned += () => ToggleStartNextWaveButton(false);

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
        Spawner.OnToggleAutoSpawn -= SetToggleAutoSpawnButtonState;
        Spawner.OnWaveCleared -= (wave) => ToggleStartNextWaveButton(true);
        Spawner.OnWaveSpawned -= () => ToggleStartNextWaveButton(false);

        BuildingManager.OnTowerPlaced -= infoPanel.SetSelectedTower;
        BuildingManager.OnDeselectTower -= infoPanel.DeselectSelectedTower;
    }

    void Setup()
    {
        gameOverUI.gameObject.SetActive(false);
        gameWonUI.gameObject.SetActive(false);

        OnVolumeChanged?.Invoke(volumeControl.value);
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

    public void ToggleStartNextWaveButton(bool showStartNextWaveButton)
    {
        // Don't show the start next wave button if auto spawn is enabled
        if (Spawner.instance.autoSpawnEnabled)

        {
            showStartNextWaveButton = false;
        }

        startNextWaveButton.gameObject.SetActive(showStartNextWaveButton);
        toggleGameSpeedButton.gameObject.SetActive(!showStartNextWaveButton);
    }

    void SetToggleGameSpeedButtonState(int gameSpeed)
    {
        toggleGameSpeedButton.GetComponent<Image>().color = gameSpeed > 1 ? Color.white : Color.gray;
    }

    void SetToggleAutoSpawnButtonState(bool active)
    {
        toggleAutoSpawnButton.GetComponent<Image>().color = active ? Color.white : Color.gray;
    }

    public void UpdateVolume()
    {
        OnVolumeChanged?.Invoke(volumeControl.value);
    }
}
