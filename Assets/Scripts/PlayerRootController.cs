using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerInput playerInput;

    [Header("Important Components")]
    [SerializeField] private GameObject Mech_Object;
    [SerializeField] private GameObject Standard_Humanoid_Object;
    private IInputAcceptor MechControls;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.MechControls.Enable();

        TakeControlOfMech();
    }

    // initiate control over the mech
    private void TakeControlOfMech()
    {
        // enable mech controls
        playerInput.MechControls.Enable();

        // set interfaces to mech
        MechControls = Mech_Object.GetComponent<IInputAcceptor>();

        MechControls.DebugCheck();

        // send the player's input to Mover
        playerInput.MechControls.LateralMove.performed += ctx => MechControls.OnMove(ctx);
        playerInput.MechControls.LateralMove.canceled += ctx => MechControls.OnMove(ctx);

        playerInput.MechControls.Jump.performed += ctx => MechControls.OnJump(ctx);
        playerInput.MechControls.Jump.canceled += ctx => MechControls.OnJump(ctx);

        playerInput.MechControls.ToggleStrafeThrusters.performed += ctx => MechControls.OnToggleStrafeThrusters(ctx);
        playerInput.MechControls.ToggleStrafeThrusters.canceled += ctx => MechControls.OnToggleStrafeThrusters(ctx);

        // send the player's input to Aimer
        playerInput.MechControls.Aim.performed += ctx => MechControls.OnGamepadLook(ctx);
        playerInput.MechControls.Aim.canceled += ctx => MechControls.OnGamepadLook(ctx);

        playerInput.MechControls.ToggleTargetFocus.performed += ctx => MechControls.OnTarget(ctx);
        playerInput.MechControls.ToggleTargetFocus.canceled += ctx => MechControls.OnTarget(ctx);

        // send the player's input to Arsenal
        playerInput.MechControls.RightMainWeaponFire.performed += ctx => MechControls.OnRightAttack(ctx);
        playerInput.MechControls.RightMainWeaponFire.canceled += ctx => MechControls.OnRightAttack(ctx);

        playerInput.MechControls.RightMainWeaponReload.performed += ctx => MechControls.OnRightReload(ctx);
        playerInput.MechControls.RightMainWeaponReload.canceled += ctx => MechControls.OnRightReload(ctx);

        playerInput.MechControls.LeftMainWeaponFire.performed += ctx => MechControls.OnLeftAttack(ctx);
        playerInput.MechControls.LeftMainWeaponFire.canceled += ctx => MechControls.OnLeftAttack(ctx);

        playerInput.MechControls.LeftMainWeaponReload.performed += ctx => MechControls.OnLeftReload(ctx);
        playerInput.MechControls.LeftMainWeaponReload.canceled += ctx => MechControls.OnLeftReload(ctx);

        playerInput.MechControls.UseEquipmentOne.performed += ctx => MechControls.OnLeftShoulder(ctx);
        playerInput.MechControls.UseEquipmentOne.canceled += ctx => MechControls.OnLeftShoulder(ctx);

        playerInput.MechControls.UseEquipmentTwo.performed += ctx => MechControls.OnRightShoulder(ctx);
        playerInput.MechControls.UseEquipmentTwo.canceled += ctx => MechControls.OnRightShoulder(ctx);

        playerInput.MechControls.Melee.performed += ctx => MechControls.OnMelee(ctx);
        playerInput.MechControls.Melee.canceled += ctx => MechControls.OnMelee(ctx);
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
