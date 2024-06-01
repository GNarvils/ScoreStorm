using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleS : BaseState
{
    public override void EnterState(CharacterMovement movement) {
    
    }

    public override void UpdateState(CharacterMovement movement)
    {
        //Pārbauda vai spēlētājs kustās
        if (movement.direction.magnitude > 0.1f) {
            if (Input.GetKey(KeyBinds.manager.run)) movement.SwitchState(movement.sprint); //Ja spēlētājs tur skriešans taustiņu, tad skrien, bet ja nē tad staigā
            else movement.SwitchState(movement.walk);
        }

    }
}
