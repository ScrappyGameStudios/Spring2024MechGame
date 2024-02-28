using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IWeaponInput
{
    void OnWeaponAttack(InputAction.CallbackContext context);
    void OnWeaponReload(InputAction.CallbackContext context);
    void OnWeaponAltAttack(InputAction.CallbackContext context);
    void OnWeaponAltReload(InputAction.CallbackContext context);
}
