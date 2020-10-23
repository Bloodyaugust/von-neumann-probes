using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]
public class SOWeapon : ScriptableObject {
    public float reload;
    public SOProjectile projectile;
}
