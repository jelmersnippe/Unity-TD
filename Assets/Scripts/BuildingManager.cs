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

    public Tower towerToPlace;

    [SerializeField]
    Placeholder placeholder;

    Placeholder activePlaceholder;

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

        TowerSelection();

        if (towerToPlace == null)
        {
            return;
        }

        if (activePlaceholder != null && towerToPlace.cost > purchaseCurrency)
        {
            UnsetTowerToPlace();
            return;
        }

        if (activePlaceholder == null && towerToPlace != null && purchaseCurrency >= towerToPlace.cost)
        {
            activePlaceholder = Instantiate(placeholder, transform);
            activePlaceholder.setBlockedLayers(blockedLayers);
        } 

        if (Input.GetMouseButtonDown(0) && activePlaceholder != null && !Physics2D.IsTouchingLayers(activePlaceholder.GetComponent<BoxCollider2D>(), blockedLayers))
        {
            PlaceTower();
        }
    }

    void TowerSelection()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnsetTowerToPlace();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && towers.Length > 0)
        {
            towerToPlace = towers[0];
        }

    }

    void UnsetTowerToPlace()
    {
        towerToPlace = null;
        if (activePlaceholder != null)
        {
            Destroy(activePlaceholder.gameObject);
        }
    }

    void PlaceTower()
    {
        if (towerToPlace.cost > purchaseCurrency || activePlaceholder == null)
        {
            return;
        }

        purchaseCurrency -= towerToPlace.cost;
        currencyUI.text = "Currency: " + purchaseCurrency.ToString();

        Instantiate(towerToPlace, activePlaceholder.transform.position, towerToPlace.transform.rotation, transform);
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
