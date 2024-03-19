using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase : MonoBehaviour
{
    #region Input
    public void OnWeaponAction(InputAction.CallbackContext context) { }
    public void OnWeaponReload(InputAction.CallbackContext context) { }
    public void OnWeaponAltAction(InputAction.CallbackContext context) { }
    public void OnWeaponAltReload(InputAction.CallbackContext context) { }
    public void OnSideAction(InputAction.CallbackContext context) { }
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
