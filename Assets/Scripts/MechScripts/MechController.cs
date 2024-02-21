using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechController : MonoBehaviour, IInputAcceptor
{
    private IMoveInput MechMove;
    private IAimInput MechAim;
    private ILoadoutInput MechLoadout;

    #region IMoveInput

    public void OnDash(InputAction.CallbackContext context)
    {
        MechMove?.OnDash(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        MechMove?.OnJump(context);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MechMove?.OnMove(context);
    }

    public void OnRushThrust(InputAction.CallbackContext context)
    {
        MechMove?.OnRushThrust(context);
    }

    public void OnToggleStrafeThrusters(InputAction.CallbackContext context)
    {
        MechMove?.OnToggleStrafeThrusters(context);
    }

    public void SetGrounded(bool state)
    {
        MechMove?.SetGrounded(state);
    }

    public void ForceGrounded(bool state)
    {
        MechMove?.ForceGrounded(state);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #endregion

    #region IAimInput
    
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        MechAim?.OnMouseLook(context);
    }

    public void OnGamepadLook(InputAction.CallbackContext context)
    {
        MechAim?.OnGamepadLook(context);
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        MechAim?.OnTarget(context);
    }

    #endregion

    #region ILoadoutInput

    public void OnRightAttack(InputAction.CallbackContext context)
    {
        MechLoadout?.OnRightAttack(context);
    }

    public void OnRightReload(InputAction.CallbackContext context)
    {
        MechLoadout?.OnRightReload(context);
    }

    public void OnLeftAttack(InputAction.CallbackContext context)
    {
        MechLoadout?.OnLeftAttack(context);
    }

    public void OnLeftReload(InputAction.CallbackContext context)
    {
        MechLoadout?.OnLeftReload(context);
    }

    public void OnRightShoulder(InputAction.CallbackContext context)
    {
        MechLoadout?.OnRightShoulder(context);
    }

    public void OnLeftShoulder(InputAction.CallbackContext context)
    {
        MechLoadout?.OnLeftShoulder(context);
    }

    public void OnMelee(InputAction.CallbackContext context)
    {
        MechLoadout?.OnMelee(context);
    }

    #endregion

    private void Awake()
    {
        MechMove = GetComponent<IMoveInput>();
        MechAim = GetComponent<IAimInput>();
        MechLoadout = GetComponent<ILoadoutInput>();
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
