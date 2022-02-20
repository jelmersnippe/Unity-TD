using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Monster", order = 2)]
public class MonsterBlueprint : ScriptableObject
{
    public Sprite sprite;
    public int health;
    public float speed;
    public int damage;
    public int currencyToDrop;
}
