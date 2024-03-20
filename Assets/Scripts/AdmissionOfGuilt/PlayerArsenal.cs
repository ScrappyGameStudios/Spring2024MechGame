using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerArsenal : MonoBehaviour
{
    private PlayerController pc;

    #region Player UI
    [Header("Resource Bars")]
    [SerializeField]
    private ResourceBar FuelBar;
    [SerializeField]
    private ResourceBar ExtendedFuelBar;

    [Header("Combat Interface")]
    [SerializeField]
    private GameObject NormalCrosshair;
    [SerializeField]
    private GameObject AimedCrosshair;
    [SerializeField]
    private GameObject WeaponDisplay;
    #endregion Player UI

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
    private float fuelMax = 2.0f;
    [SerializeField]
    private float refuelCooldown = 1.5f;
    [SerializeField]
    private float refuelRate = 0.5f;
    [SerializeField]
    private bool mustResetOnEmpty = true;
    [SerializeField]
    private float pickupRefuel = 0.5f;

    [Header("Extended Tanks")]
    [SerializeField]
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
    private float dashCooldown = 0.8f;
    [SerializeField]
    private float dashFuelCost = 1.0f;

    [Header("Rush Info")]
    [SerializeField]
    private float rushSpeed = 8.0f;
    [SerializeField]
    private float rushStartup = 1.0f;
    [SerializeField]
    private float rushCooldown = 1.5f;
    [SerializeField]
    private float rushFuelCost = 0.3f;

    private float fuelPause;
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
        if (!CheckIfFuelExists()) return;
        // can we dash?
        if (pc.SetMoveAbility(MoveAbility.Dash, dashTime, dashSpeed))
        {
            // set fuel timer
            fuelPause = dashCooldown;

            ExpendFuel(dashFuelCost);
        }
    }
    public void OnRush(InputAction.CallbackContext context)
    {
        if (!CheckIfFuelExists()) return;
        // can we rush?
        if (context.performed && Thruster == ThrusterEquipment.RushThrusters)
        {
            if (pc.SetMoveAbility(MoveAbility.Rush, rushStartup, rushSpeed))
            {

            }
        }
    }
    public void OnToggleStrafe(InputAction.CallbackContext context)
    {
        if (context.performed && Leg == LegEquipment.StrafeThrusters)
        {
            if (!pc.SetMoveMode(MoveMode.StrafeThrusters)) pc.SetMoveMode(MoveMode.Standard);
        }
    }
    #endregion Movement Input

    // Start is called before the first frame update
    void Start()
    {
        // fuel initialization
        fuelCurrent = fuelMax;
        extendedCurrent = extendedMax;
        FuelBar.ShowResourceBar();
        FuelBar.UpdateResourceBar(fuelMax, fuelCurrent);
        switch (Back)
        {
            case BackEquipment.BackupFuelTanks:
                ExtendedFuelBar.ShowResourceBar();
                break;
            default:
                ExtendedFuelBar.HideResourceBar();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fuelCurrent < fuelMax) FuelUpdate();
    }

    #region Fuel
    private void FuelUpdate()
    {
        if (fuelPause > 0f)
        {
            // countdown to refuel
            fuelPause -= Time.deltaTime;
        }
        else
        {
            // refuel over time
            fuelCurrent = Mathf.Clamp(fuelCurrent + Time.deltaTime * refuelRate,0f,fuelMax);
            FuelBar.UpdateResourceBar(fuelMax, fuelCurrent);
        }
    }

    private bool CheckIfFuelExists()
    {
        // check appropriate places
        switch (Back)
        {
            case BackEquipment.BackupFuelTanks:
                // is there fuel? does it still need a full reset
                if (!needReset && (fuelCurrent + extendedCurrent) > 0f) return true;
                break;
            default:
                // is there fuel?
                if (!needReset && fuelCurrent > 0f) return true;
                break;
        }

        return false;
    }
    private bool CheckForMaxFuel()
    {
        // check appropriate places
        switch (Back)
        {
            case BackEquipment.BackupFuelTanks:
                // is there fuel? does it still need a full reset
                if (fuelCurrent < fuelMax || extendedCurrent < extendedMax) return false;
                break;
            default:
                // is there fuel?
                if (fuelCurrent < fuelMax) return false;
                break;
        }

        return true;
    }

    private void ExpendFuel(float amount)
    {
        // check appropriate sources
        switch (Back)
        {
            case BackEquipment.BackupFuelTanks:
                // use fuel
                if (fuelCurrent > amount)
                {
                    // only take from fuel
                    fuelCurrent -= amount;
                }
                else if (extendedCurrent > (amount - fuelCurrent))
                {
                    // only extended has fuel
                    extendedCurrent -= (amount - fuelCurrent);
                    fuelCurrent = 0f;
                }
                else
                {
                    // tanks empty
                    extendedCurrent = 0f;
                    fuelCurrent = 0f;

                    // need reset?
                    needReset = mustResetOnEmpty;
                }
                break;
            default:
                if (fuelCurrent > amount)
                {
                    // only take from fuel
                    fuelCurrent -= amount;
                }
                else
                {
                    // tank empty
                    fuelCurrent = 0f;

                    // need reset?
                    needReset = mustResetOnEmpty;
                }
                break;
        }

        FuelBar.UpdateResourceBar(fuelMax, fuelCurrent);
        ExtendedFuelBar.UpdateResourceBar(extendedMax, extendedCurrent);
    }

    private void AddFuel()
    {
        // check appropriate places
        switch (Back)
        {
            case BackEquipment.BackupFuelTanks:
                // check backup first
                if (extendedCurrent < extendedMax) extendedCurrent = Mathf.Clamp(extendedCurrent + pickupRefuel,0f,extendedMax);
                break;
            default:
                // is there fuel?
                if (!needReset && fuelCurrent > 0f) ;
                break;
        }
    }
    #endregion Fuel

    public bool PickupResource(PickupTier tier, bool mega)
    {
        switch (tier)
        {
            case PickupTier.Fuel:
                if (true) { }
                break;
            case PickupTier.Ammo:
                break;
            case PickupTier.Mixed:
                break;
        }
        return false;
    }
}

#region enumerators
public enum PickupTier
{
    None,
    Fuel,
    Ammo,
    Mixed,
    Size
}
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