using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnnamedWeaponConfig", menuName = "WeaponConfig")]
public class WeaponConfigSO : ScriptableObject
{
    [SerializeField] private float maxAmmo;
    [SerializeField] private float damage;
    [SerializeField] private bool areaDamage;

    public float MaxAmmo => maxAmmo;
    public float Damage => damage;
    public bool AreaDamage => areaDamage;
}