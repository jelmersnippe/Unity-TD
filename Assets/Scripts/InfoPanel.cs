using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;

    [SerializeField]
    Tower selectedTower;

    [SerializeField]
    Image image;

    [SerializeField]
    TextMeshProUGUI damageText;

    [SerializeField]
    TextMeshProUGUI rangeText;

    [SerializeField]
    TextMeshProUGUI fireRateText;

    [SerializeField]
    GameObject infoSection;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetSelectedTower(Tower tower)
    {
        selectedTower = tower;

        image.sprite = tower.uiSprite;
        damageText.text = tower.damage.ToString();
        rangeText.text = tower.range.ToString();
        fireRateText.text = tower.roundsPerMinute.ToString();

        infoSection.SetActive(true);

    }

    public void DeselectSelectedTower()
    {
        selectedTower = null;
        infoSection.SetActive(false);
    }
}
