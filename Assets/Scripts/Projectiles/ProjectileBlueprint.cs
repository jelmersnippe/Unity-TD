using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Projectile", order = 3)]
public class ProjectileBlueprint : ScriptableObject
{
    public Projectile prefab;
    public Sprite sprite;
    public float speed = 20f;
    public bool homing = false;
}
