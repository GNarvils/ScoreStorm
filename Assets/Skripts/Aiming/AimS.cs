using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimS : AimBaseState
{
    //Ieiet mērķēšanas stāvoklī
    public override void EnterState(CameraAim aim) {
        //Pietuvina kameru un iestata vērtību, ja animācija varētu spēlēt
        aim.currentFov = aim.aimFov;
        aim.anim.SetBool("Aiming", true);
    }
    public override void UpdateState(CameraAim aim) {
        //Ja atlaiž labo peles taustiņu pāriet uz nemērķēšanas stāvokļa
        if (Input.GetKeyUp(KeyCode.Mouse1)) { 
            aim.SwitchState(aim.Hip); 
        }
    }
}
