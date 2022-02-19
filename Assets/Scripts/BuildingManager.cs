using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    public static event Action<TowerController> OnTowerPlaced;
    public static event Action OnDeselectTower;

    public TowerController selectedTower;

    public TowerController towerToPlace;

    [SerializeField] LayerMask blockedLayers;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnDeselectTower?.Invoke();
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
                if (hit.collider != null && hit.collider.gameObject.GetComponent<TowerController>() != null)
                {
                    SetSelectedTower(hit.collider.gameObject.GetComponent<TowerController>());
                }
            }
        }
    }

    public void SetSelectedTower(TowerController tower)
    {
        DeselectCurrentTower();
        selectedTower = tower;
        tower.ShowRange(true);
        InfoPanel.instance.SetSelectedTower(selectedTower);
    }

    public void SetTowerToPlace(TowerController tower)
    {
        CancelPlacingTower();
        DeselectCurrentTower();

        InfoPanel.instance.SetSelectedTower(tower);

        towerToPlace = Instantiate(tower, transform);
        towerToPlace.GetComponent<Placeholder>().setBlockedLayers(blockedLayers);
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
        if (towerToPlace != null && towerToPlace.cost > GameManager.instance.purchaseCurrency)
        {
            return;
        }

        OnTowerPlaced?.Invoke(towerToPlace);
        towerToPlace.ConvertToActive();
        towerToPlace.gameObject.layer = LayerMask.NameToLayer("Tower");

        SetSelectedTower(towerToPlace);

        towerToPlace = null;
    }
}
