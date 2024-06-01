﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeathState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        //Roku svaru uzliek uz 0, lai varētu spēlēt animāciju
        actions.rHandAim.weight = 0;
        actions.lHandIK.weight = 0;
        actions.anim.SetTrigger("Dead");
        Debug.Log("Dead");

    }

    public override void UpdateState(ActionStateManager actions)
    {

    }
}
