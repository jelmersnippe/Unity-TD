using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    Tower towerToPlace;

    [SerializeField]
    Placeholder placeholder;

    Placeholder activePlaceholder;

    void Update()
    {
        if (towerToPlace == null)
        {
            return;
        }

        if (activePlaceholder == null)
        {
            activePlaceholder = Instantiate(placeholder, transform);
            activePlaceholder.setTowerToPlace(towerToPlace);  
        }
    }
}
