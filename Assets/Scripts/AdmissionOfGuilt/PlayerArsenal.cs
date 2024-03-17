using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArsenal : MonoBehaviour
{
    #region Arsenal
    // replace below with unique scripts
    // universal weapon scripts and unique for each equipment type
    [Header("Equipped Weapons")]
    [SerializeField]
    private WeaponBase WeaponOne;
    [SerializeField]
    private WeaponBase WeaponTwo;

    [SerializeField]
    private WeaponBase LeftSideWeapon;
    [SerializeField]
    private WeaponBase RightSideWeapon;

    [Header("Equipped Equipment")]
    [SerializeField]
    public BackEquipment Back;
    [SerializeField]
    public LegEquipment Leg;
    [SerializeField]
    public ArmorEquipment Armor;
    [SerializeField]
    public ThrusterEquipment Thruster;
    #endregion Arsenal

    #region MovementAbilities
    [Header("Fuel Resource")]
    [SerializeField]
    private float fuelMax = 1.0f;
    [SerializeField]
    private float refuelCooldown = 1.5f;
    [SerializeField]
    private float refuelRate = 0.5f;
    [SerializeField]
    private bool mustResetOnEmpty = true;

    private float fuelCurrent;

    [Header("Dash Info")]
    [SerializeField]
    private float dashTime = 0.5f;
    [SerializeField]
    private float dashCooldown = 0.5f;
    [SerializeField]
    private float fuelCost = 0.5f;
    #endregion MovementAbilities

    #region Input
    // update below after implementing weapon/equipment scripts
    // pass on to selected weapon
    public void OnWeaponAction(InputAction.CallbackContext context) { }
    public void OnWeaponAltAction(InputAction.CallbackContext context) { }
    public void OnWeaponReload(InputAction.CallbackContext context) { }
    public void OnWeaponAltReload(InputAction.CallbackContext context) { }
    // update to use side weapons
    public void OnLeftSideAction(InputAction.CallbackContext context) { }
    public void OnRightSideAction(InputAction.CallbackContext context) { }
    public void OnDash(InputAction.CallbackContext context) { }
    public void OnRush(InputAction.CallbackContext context) { }
    public void OnToggleStrafe(InputAction.CallbackContext context) { }
    #endregion Input

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

#region enumerators
public enum BackEquipment
{
    None,
    ShieldGenerator,
    BackupFuelTanks,
    AmmoPrinter,
    Size
}
public enum LegEquipment
{
    None,
    CaprineAscenders,
    StrafeThrusters,
    SeismicDampers,
    Size
}
public enum ArmorEquipment
{
    None,
    ReactivePlating,
    InsulatedSleeving,
    Size
}
public enum ThrusterEquipment
{
    None,
    RushThrusters,
    OverfueledNozzles,
    Size
}
#endregion