using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{
    public enum Type
    {
        None,

        Default_DamageUp,
        Default_PierceUp,
        Default_FireRateUp,

        Splash_RadiusUp,
        Splash_TargetCountUp,

        Basic_TripleShot,

        Gattling_ConsecutiveDamageUp,
        Gattling_MaxFireRateUp,
        Gattling_QuickerWindup,

        Shotgun_ConcentratedPellets,
        Shotgun_MorePellets,
        Shotgun_ReduceSpread,
    }

    public Type type;
    public Sprite sprite;
    public string displayName;
    public string description;
    public int cost;
    public int depth;
    public Type parentUpgrade;
}
