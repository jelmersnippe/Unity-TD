using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    [SerializeField] TextMeshProUGUI healthUI;
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

    [SerializeField] TextMeshProUGUI currencyUI;
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

    public Tower selectedTower;

    public Placeholder towerToPlace;

    [SerializeField]
    LayerMask blockedLayers;

    bool gameOver = false;
    bool gameWon = false;

    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas gameWonUI;

    [SerializeField] int towerLayer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Time.timeScale = 1;
    }

    private void Start()
    {
        health = startingHealth;
        purchaseCurrency = startingCurrency;
    }

    void Update()
    {
        Debug.Log(Time.timeScale);
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedTower != null)
            {
                DeselectCurrentTower();
            }
            if (towerToPlace != null)
            {
                CancelPlacingTower();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (towerToPlace != null && !Physics2D.IsTouchingLayers(towerToPlace.GetComponent<Collider2D>(), blockedLayers))
            {
                PlaceTower();
            }
            else if (towerToPlace == null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<Tower>() != null)
                {
                    SetSelectedTower(hit.collider.gameObject.GetComponent<Tower>());
                }
            }
        }
    }

    public void SetSelectedTower(Tower tower)
    {
        DeselectCurrentTower();
        selectedTower = tower;
        tower.ShowRange(true);
        InfoPanel.instance.SetSelectedTower(selectedTower);
    }

    public void SetTowerToPlace(Tower tower)
    {
        CancelPlacingTower();
        DeselectCurrentTower();
        Placeholder placeholder = tower.GetComponent<Placeholder>();

        InfoPanel.instance.SetSelectedTower(tower);

        towerToPlace = Instantiate(placeholder, transform);
        towerToPlace.setBlockedLayers(blockedLayers);
    }

    void DeselectCurrentTower()
    {
        InfoPanel.instance.DeselectSelectedTower();
        if (selectedTower != null)
        {
            selectedTower.ShowRange(false);
        }
    }

    void CancelPlacingTower()
    {
        InfoPanel.instance.DeselectSelectedTower();
        if (towerToPlace != null)
        {
            Destroy(towerToPlace.gameObject);
        }
    }

    void PlaceTower()
    {
        if (towerToPlace != null && towerToPlace.cost > purchaseCurrency)
        {
            return;
        }

        purchaseCurrency -= towerToPlace.cost;
        currencyUI.text = "Currency: " + purchaseCurrency.ToString();
        towerToPlace.ConvertToActiveTower();
        towerToPlace.gameObject.layer = towerLayer;

        SetSelectedTower(towerToPlace.GetComponent<Tower>());

        towerToPlace = null;
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
}
