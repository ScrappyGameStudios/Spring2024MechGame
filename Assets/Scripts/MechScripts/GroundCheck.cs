using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private IMoveInput mover;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == mover.GetGameObject())
            return;

        mover.SetGrounded(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == mover.GetGameObject())
            return;

        mover.SetGrounded(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == mover.GetGameObject())
            return;

        mover.SetGrounded(true);
    }
}
