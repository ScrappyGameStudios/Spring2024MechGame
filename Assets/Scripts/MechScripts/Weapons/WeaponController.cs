using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("Necessary References")]
    [SerializeField] private Transform MuzzlePoint;
    [SerializeField] private Animator GunAnimator;

    [Header("Attack/Ammo References")]

    private bool wantToAttack;
    private bool wantToReload;
    private bool wantToAltAttack;
    private bool wantToAltReload;

    #region IWeaponInput

    public void OnWeaponAttack(InputAction.CallbackContext context)
    {
        wantToAttack = context.ReadValueAsButton();
    }

    public void OnWeaponReload(InputAction.CallbackContext context)
    {
        wantToReload = context.ReadValueAsButton();
    }

    public void OnWeaponAltAttack(InputAction.CallbackContext context)
    {
        wantToAltAttack = context.ReadValueAsButton();
    }

    public void OnWeaponAltReload(InputAction.CallbackContext context)
    {
        wantToAltReload = context.ReadValueAsButton();
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
