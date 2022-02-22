using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;

    public Projectile basicProjectile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
