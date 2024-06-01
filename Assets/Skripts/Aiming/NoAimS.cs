using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAimS : AimBaseState
{

    //Ieiet stāvoklī
    public override void EnterState(CameraAim aim) {
        //Uzstaisa redzes lauku par parasto redzes lauku
        aim.currentFov = aim.Fov;
        aim.anim.SetBool("Aiming", false);
    }
    public override void UpdateState(CameraAim aim) {

        //Ja nospies labo peles tausiņu pāriet uz mērķēšanas stāvokļā.
        if (Input.GetKey(KeyCode.Mouse1)) {
            aim.SwitchState(aim.Aim); 
        }
    }
}
