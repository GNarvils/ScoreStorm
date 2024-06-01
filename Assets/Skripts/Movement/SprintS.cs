using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SprintS : BaseState
{

    public override void EnterState(CharacterMovement movement)
    {
        //Dara animāciju
        movement.anim.SetBool("Sprint", true);
    }

    public override void UpdateState(CharacterMovement movement)
    {
        if (Input.GetKeyUp(KeyBinds.manager.run)) ExitState(movement, movement.walk);//Ja atlaiž vaļā skriešanas pogu, tad staigā
        else if (movement.direction.magnitude < 0.1f) ExitState(movement, movement.idle); //Ja nekustas, tad pāriet stāvēšanas stāvoklī

        if (movement.vInput < 0) movement.speed = movement.sprintBackwardsS; //Ja skrien uz atpakaļu ātrums samazinās
        else movement.speed = movement.sprintSpeed;


    }

    void ExitState(CharacterMovement movement, BaseState state)
    {
        //Ja iziet no stāvokļa beidz animāciju
        movement.anim.SetBool("Sprint", false);
        movement.SwitchState(state);
    }
}
