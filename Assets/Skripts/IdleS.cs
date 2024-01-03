using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleS : BaseState
{
    public override void EnterState(CharacterMovement movement) {
    
    }

    public override void UpdateState(CharacterMovement movement)
    {
        if (movement.direction.magnitude > 0.1f) {
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.sprint);
            else movement.SwitchState(movement.walk);
        }
    }
}
