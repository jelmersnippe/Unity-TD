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

    [SerializeField]
    GameObject upgradeSection;

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
        UpdateInfo();
    }

    public void DeselectSelectedTower()
    {
        selectedTower = null;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (selectedTower == null)
        {
            infoSection.SetActive(false);
            upgradeSection.SetActive(false);
            return;
        }

        image.sprite = selectedTower.uiSprite;
        damageText.text = selectedTower.damage.ToString();
        rangeText.text = selectedTower.range.ToString();
        fireRateText.text = selectedTower.roundsPerMinute.ToString();

        infoSection.SetActive(true);
        upgradeSection.SetActive(selectedTower.isActiveAndEnabled);
    }
}
