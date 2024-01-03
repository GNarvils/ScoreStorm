using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkS : BaseState
{
    public override void EnterState(CharacterMovement movement)
    {

    }

    public override void UpdateState(CharacterMovement movement)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.sprint);
        else if(movement.direction.magnitude<0.1f) ExitState(movement, movement.idle);

        if (movement.vInput < 0) movement.speed = movement.walkBackwardsS;
        else movement.speed = movement.walkSpeed;
    }

    void ExitState(CharacterMovement movement, BaseState state) {
        movement.SwitchState(state);
    }
}
