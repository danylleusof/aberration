using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Weapon", menuName ="Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage, maxDistance;

    [Header("Reloading")]
    public int currentAmmo, magazineSize;
    public float fireRate, reloadTime;
    [HideInInspector]
    public bool reloading;
}
