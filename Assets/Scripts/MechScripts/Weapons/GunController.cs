using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour, IWeaponInput
{
    [Header("Necessary References")]
    [SerializeField] private Transform MuzzlePoint;
    [SerializeField] private Animator GunAnimator;

    [Header("Attack/Ammo References")]

    private bool wantToAttack;
    private bool wantToReload;

    public void OnWeaponAttack(InputAction.CallbackContext context)
    {
        if (context.performed) wantToAttack = true;
        else if (context.performed) wantToAttack = false;
    }

    public void OnWeaponReload(InputAction.CallbackContext context)
    {
        if (context.performed) wantToReload = true;
        else if (context.performed) wantToReload = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
