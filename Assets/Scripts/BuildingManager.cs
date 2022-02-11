using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    [SerializeField]
    int startingHealth;

    [SerializeField]
    int health;

    [SerializeField]
    Tower[] towers;

    private Tower selectedTower;

    public Placeholder towerToPlace;

    [SerializeField]
    int purchaseCurrency;

    [SerializeField]
    LayerMask blockedLayers;

    bool gameOver = false;

    [SerializeField]
    Canvas gameOverUI;

    [SerializeField]
    TextMeshProUGUI currencyUI;

    [SerializeField]
    TextMeshProUGUI healthUI;

    [SerializeField]
    int towerLayer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        health = startingHealth;
        currencyUI.text = "Currency: " + purchaseCurrency.ToString();
        healthUI.text = "Health: " + health.ToString();
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.anyKey)
            {
                gameOverUI.gameObject.SetActive(false);
                Time.timeScale = 1;
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

    public void addPurchaseCurrency(int currency)
    {
        if (currency < 0)
        {
            return;
        }
        purchaseCurrency += currency;
        currencyUI.text = "Currency: " + purchaseCurrency.ToString();
    }

    public void takeDamage(int damage)
    {
        health = Mathf.Max(health -= damage, 0);
        healthUI.text = "Health: " + health.ToString();

        if (health <= 0)
        {
            gameOver = true;
            gameOverUI.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
