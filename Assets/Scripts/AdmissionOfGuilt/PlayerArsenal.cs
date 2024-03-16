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
    private List<GameObject> ActiveWeapons;

    [SerializeField]
    private GameObject LeftSideWeapon;
    [SerializeField]
    private GameObject RightSideWeapon;

    [SerializeField]
    private GameObject BackEquipment;
    [SerializeField]
    private GameObject LegEquipment;
    [SerializeField]
    private GameObject ArmorEquipment;
    [SerializeField]
    private GameObject ThrusterEquipment;
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
    public void OnWeaponAction(InputAction.CallbackContext context) { }
    public void OnWeaponAltAction(InputAction.CallbackContext context) { }
    public void OnWeaponReload(InputAction.CallbackContext context) { }
    public void OnWeaponAltReload(InputAction.CallbackContext context) { }
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
