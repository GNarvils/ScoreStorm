using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimS : AimBaseState
{

    public override void EnterState(CameraAim aim) {
        aim.currentFov = aim.aimFov;
        aim.anim.SetBool("Aiming", true);
    }
    public override void UpdateState(CameraAim aim) {
        if (Input.GetKeyUp(KeyCode.Mouse1)) { 
            aim.redDot.SetActive(false);
            aim.blackDot.SetActive(true);
            aim.SwitchState(aim.Hip); 
        }
    }
}
