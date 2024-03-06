using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechLoadout : MonoBehaviour, ILoadoutInput
{

    [Header("Important Components")]
    [SerializeField] private GameObject RightWeapon;
    [SerializeField] private GameObject LeftWeapon;
    [SerializeField] private GameObject RightShoulder;
    [SerializeField] private GameObject LeftShoulder;
    private IWeaponInput ActiveWeaponInput;
    private IWeaponInput RightShoulderInput;
    private IWeaponInput LeftShoulderInput;

    [Header("Repair Functionality")]
    [SerializeField] private int startingPacks;
    [SerializeField] private float repairDelay;
    [SerializeField] private float repairAmount;
    private float repairTime;
    private int packsRemaining;

    #region ILoadoutInput

    private void Awake()
    {
        
    }

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        ActiveWeaponInput?.OnWeaponAttack(context);
    }

    public void OnRightReload(InputAction.CallbackContext context)
    {
        ActiveWeaponInput?.OnWeaponReload(context);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        ActiveWeaponInput?.OnWeaponAltAttack(context);
    }

    public void OnLeftReload(InputAction.CallbackContext context)
    {
        ActiveWeaponInput?.OnWeaponAltReload(context);
    }

    public void OnRightShoulder(InputAction.CallbackContext context)
    {
        RightShoulderInput?.OnWeaponAttack(context);
    }

    public void OnLeftShoulder(InputAction.CallbackContext context)
    {
        LeftShoulderInput?.OnWeaponAttack(context);
    }

    public void OnRepair(InputAction.CallbackContext context)
    {
        if (repairTime > 0 && packsRemaining > 0)
        {
            Repair(); 
        }
    }

    public void OnMelee(InputAction.CallbackContext callback)
    {
        // I dunno, figure it out later
    }

    public void DebugCheck()
    {
        Debug.Log("MechLoadout found!");
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (repairTime > 0f) repairTime -= Time.deltaTime;
    }

    private void Repair()
    {
        // heal in the IStatusHandler

        repairTime = repairDelay;
        packsRemaining--;
    }
}
