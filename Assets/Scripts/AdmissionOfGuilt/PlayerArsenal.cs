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

    private WeaponSlot ActiveSlot;

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

    [Header("Extended Tanks")]
    private float extendedMax = 1.0f;

    private bool needReset = false;
    private float fuelCurrent;
    private float extendedCurrent;

    [Header("Dash Info")]
    [SerializeField]
    private float dashSpeed = 10.0f;
    [SerializeField]
    private float dashTime = 0.5f;
    [SerializeField]
    private float dashCooldown = 0.5f;
    [SerializeField]
    private float dashFuelCost = 0.5f;

    [Header("Rush Info")]
    [SerializeField]
    private float rushSpeed = 8.0f;
    [SerializeField]
    private float rushStartup = 1.0f;
    [SerializeField]
    private float rushCooldown = 1.5f;
    [SerializeField]
    private float rushFuelCost = 0.3f;
    #endregion MovementAbilities

    #region Weapon Input
    // update below after implementing weapon/equipment scripts
    // pass on to selected weapon
    public void OnWeaponAction(InputAction.CallbackContext context)
    {
        switch (ActiveSlot)
        {
            case WeaponSlot.One:
                WeaponOne.OnWeaponAction(context);
                break;
            case WeaponSlot.Two:
                WeaponTwo.OnWeaponAction(context);
                break;
            default:
                break;
        }
    }
    public void OnWeaponAltAction(InputAction.CallbackContext context)
    {
        switch (ActiveSlot)
        {
            case WeaponSlot.One:
                WeaponOne.OnWeaponAltAction(context);
                break;
            case WeaponSlot.Two:
                WeaponTwo.OnWeaponAltAction(context);
                break;
            default:
                break;
        }
    }
    public void OnWeaponReload(InputAction.CallbackContext context)
    {
        switch (ActiveSlot)
        {
            case WeaponSlot.One:
                WeaponOne.OnWeaponReload(context);
                break;
            case WeaponSlot.Two:
                WeaponTwo.OnWeaponReload(context);
                break;
            default:
                break;
        }
    }
    public void OnWeaponAltReload(InputAction.CallbackContext context)
    {
        switch (ActiveSlot)
        {
            case WeaponSlot.One:
                WeaponOne.OnWeaponAltReload(context);
                break;
            case WeaponSlot.Two:
                WeaponTwo.OnWeaponAltReload(context);
                break;
            default:
                break;
        }
    }
    // update to use side weapons
    public void OnLeftSideAction(InputAction.CallbackContext context) { LeftSideWeapon.OnSideAction(context); }
    public void OnRightSideAction(InputAction.CallbackContext context) { RightSideWeapon.OnSideAction(context); }
    public void OnWeaponSwap(InputAction.CallbackContext context) { }
    #endregion Weapon Input

    #region Movement Input
    public void OnDash(InputAction.CallbackContext context)
    {

    }
    public void OnRush(InputAction.CallbackContext context)
    {

    }
    public void OnToggleStrafe(InputAction.CallbackContext context) { }
    #endregion Movement Input

    // Start is called before the first frame update
    void Start()
    {
        // fuel initialization
        fuelCurrent = fuelMax;
        extendedCurrent = extendedMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

#region enumerators
public enum WeaponSlot
{
    None,
    One,
    Two,
    Size
}
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