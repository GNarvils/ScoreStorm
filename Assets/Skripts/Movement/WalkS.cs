using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkS : BaseState
{
    public override void EnterState(CharacterMovement movement)
    {
        //Dara staigāšanas animāciju
        movement.anim.SetBool("Walk", true);
    }

    public override void UpdateState(CharacterMovement movement)
    {
        if (Input.GetKey(KeyBinds.manager.run)) ExitState(movement, movement.sprint); // Ja tur skriešanas pogu tad skrien
        else if(movement.direction.magnitude<0.1f) ExitState(movement, movement.idle); //Ja nekustas, tad pāriet uz stāvēšanas stāvokli

        if (movement.vInput < 0) movement.speed = movement.walkBackwardsS; //Ja spēlētājs kustās uz atpakaļu samazina ātrumu
        else movement.speed = movement.walkSpeed;

    }

    void ExitState(CharacterMovement movement, BaseState state) {
        //Kad iziet no stāvokļa beidz animāciju
        movement.anim.SetBool("Walk", false);
        movement.SwitchState(state);
    }
}
