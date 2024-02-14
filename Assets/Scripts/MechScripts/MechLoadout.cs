using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechLoadout : MonoBehaviour, ILoadoutInput
{
    [Header("Important Components")]
    [SerializeField] private IWeaponInput RightWeapon;
    [SerializeField] private IWeaponInput LeftWeapon;
    [SerializeField] private IWeaponInput RightShoulder;
    [SerializeField] private IWeaponInput LeftShoulder;

    #region ILoadoutInput

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        RightWeapon.OnWeaponAttack(context);
    }

    public void OnRightReload(InputAction.CallbackContext context)
    {
        RightWeapon.OnWeaponReload(context);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        LeftWeapon.OnWeaponAttack(context);
    }

    public void OnLeftReload(InputAction.CallbackContext context)
    {
        LeftWeapon.OnWeaponReload(context);
    }

    public void OnRightShoulder(InputAction.CallbackContext context)
    {
        RightShoulder.OnWeaponAttack(context);
    }

    public void OnLeftShoulder(InputAction.CallbackContext context)
    {
        LeftShoulder.OnWeaponAttack(context);
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
        
    }
}
