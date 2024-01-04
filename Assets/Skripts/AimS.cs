using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimS : AimBaseState
{

    public override void EnterState(CameraAim aim) {
        aim.currentFov = aim.aimFov;
    }
    public override void UpdateState(CameraAim aim) {
        if (Input.GetKeyUp(KeyCode.Mouse1)) aim.SwitchState(aim.Hip);
    }
}
