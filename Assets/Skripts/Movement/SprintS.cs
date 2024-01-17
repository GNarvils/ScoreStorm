using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintS : BaseState
{
    public override void EnterState(CharacterMovement movement)
    {
        movement.anim.SetBool("Sprint", true);
    }

    public override void UpdateState(CharacterMovement movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.walk);
        else if (movement.direction.magnitude < 0.1f) ExitState(movement, movement.idle);

        if (movement.vInput < 0) movement.speed = movement.sprintBackwardsS;
        else movement.speed = movement.sprintSpeed;

    }

    void ExitState(CharacterMovement movement, BaseState state)
    {
        movement.anim.SetBool("Sprint", false);
        movement.SwitchState(state);
    }
}
