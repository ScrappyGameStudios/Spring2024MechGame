using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    [Header("Important Components")]
    [SerializeField] private IMoveInput Mover;
    [SerializeField] private ILoadoutInput Arsenal;

    private void Awake()
    {
        playerInput = new PlayerInput();

        // send the player's input to Mover
        playerInput.MechControls.LateralMove.performed += ctx => Mover.OnMove(ctx);
        playerInput.MechControls.LateralMove.canceled += ctx => Mover.OnMove(ctx);

        playerInput.MechControls.Jump.performed += ctx => Mover.OnJump(ctx);
        playerInput.MechControls.Jump.canceled += ctx => Mover.OnJump(ctx);

        playerInput.MechControls.ToggleStrafeThrusters.performed += ctx => Mover.OnToggleStrafeThrusters(ctx);
        playerInput.MechControls.ToggleStrafeThrusters.canceled += ctx => Mover.OnToggleStrafeThrusters(ctx);

        // send the player's input to Arsenal
        playerInput.MechControls.RightMainWeaponFire.performed += ctx => Arsenal.OnRightAttack(ctx);
        playerInput.MechControls.RightMainWeaponFire.canceled += ctx => Arsenal.OnRightAttack(ctx);

        playerInput.MechControls.RightMainWeaponReload.performed += ctx => Arsenal.OnRightReload(ctx);
        playerInput.MechControls.RightMainWeaponReload.canceled += ctx => Arsenal.OnRightReload(ctx);

        playerInput.MechControls.LeftMainWeaponFire.performed += ctx => Arsenal.OnLeftAttack(ctx);
        playerInput.MechControls.LeftMainWeaponFire.canceled += ctx => Arsenal.OnLeftAttack(ctx);

        playerInput.MechControls.LeftMainWeaponReload.performed += ctx => Arsenal.OnLeftReload(ctx);
        playerInput.MechControls.LeftMainWeaponReload.canceled += ctx => Arsenal.OnLeftReload(ctx);

        playerInput.MechControls.UseEquipmentOne.performed += ctx => Arsenal.OnLeftShoulder(ctx);
        playerInput.MechControls.UseEquipmentOne.canceled += ctx => Arsenal.OnLeftShoulder(ctx);

        playerInput.MechControls.UseEquipmentTwo.performed += ctx => Arsenal.OnRightShoulder(ctx);
        playerInput.MechControls.UseEquipmentTwo.canceled += ctx => Arsenal.OnRightShoulder(ctx);
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
