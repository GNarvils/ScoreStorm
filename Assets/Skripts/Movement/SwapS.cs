using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapS : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.anim.SetTrigger("SwapWeapon");
        actions.lHandIK.weight = 0;
        actions.rHandAim.weight = 0;
        Debug.Log("Swap");
    }

    public override void UpdateState(ActionStateManager actions)
    {

    }
}

