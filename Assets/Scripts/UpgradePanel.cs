using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] List<Upgrade> items = new List<Upgrade>();
    [SerializeField] UpgradeItem purchaseButton;

    public void setUpgrades(List<Upgrade> upgradesToSet)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float panelHeight = GetComponent<RectTransform>().sizeDelta.y;
        items = upgradesToSet;
        float itemHeight = purchaseButton.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < items.Count; i++)
        {
            Upgrade item = items[i];
            UpgradeItem createdUpgradeItem = Instantiate(purchaseButton, transform.position - new Vector3(0, (panelHeight / 2) - (itemHeight / 2) - (itemHeight * i)), Quaternion.identity, transform);
            createdUpgradeItem.SetItem(item);
        }
    }
}
