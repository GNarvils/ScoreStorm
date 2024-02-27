using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAimS : AimBaseState
{
    public override void EnterState(CameraAim aim) {
        aim.currentFov = aim.Fov;
        aim.anim.SetBool("Aiming", false);
    }
    public override void UpdateState(CameraAim aim) {
        if (Input.GetKey(KeyCode.Mouse1)) {
            aim.SwitchState(aim.Aim); 
        }
    }
}
