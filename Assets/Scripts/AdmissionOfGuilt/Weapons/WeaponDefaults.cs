using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : ScriptableObject
{
    [Header("Weapons")]
    [SerializeField]
    private WeaponBase WeaponOne;
    [SerializeField]
    private WeaponBase WeaponTwo;

    [SerializeField]
    private WeaponBase LeftSideWeapon;
    [SerializeField]
    private WeaponBase RightSideWeapon;

    [SerializeField]
    private BackEquipment Back;
    [SerializeField]
    private LegEquipment Leg;
    [SerializeField]
    private ArmorEquipment Armor;
    [SerializeField]
    private ThrusterEquipment Thruster;
}
