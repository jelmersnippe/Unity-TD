using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class UpgradeBlueprint : ScriptableObject
{
    public UpgradeType type;
    public Sprite sprite;
    public string displayName;
    public string description;
}
