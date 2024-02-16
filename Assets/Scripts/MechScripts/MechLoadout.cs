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
    private IWeaponInput RightWeaponInput;
    private IWeaponInput LeftWeaponInput;
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
        RightWeaponInput = RightWeapon.GetComponent<IWeaponInput>();
        LeftWeaponInput = LeftWeapon.GetComponent<IWeaponInput>();
        RightShoulderInput = RightShoulder.GetComponent<IWeaponInput>();
        LeftShoulderInput = LeftShoulder.GetComponent<IWeaponInput>();
    }

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        RightWeaponInput?.OnWeaponAttack(context);
    }

    public void OnRightReload(InputAction.CallbackContext context)
    {
        RightWeaponInput?.OnWeaponReload(context);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        LeftWeaponInput?.OnWeaponAttack(context);
    }

    public void OnLeftReload(InputAction.CallbackContext context)
    {
        LeftWeaponInput?.OnWeaponReload(context);
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
