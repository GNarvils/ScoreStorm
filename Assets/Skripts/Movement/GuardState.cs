using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.rHandAim.weight = 0;
        actions.lHandIK.weight = 0;
        actions.anim.SetTrigger("Guard");
        Debug.Log("Guarding");
    }

    public override void UpdateState(ActionStateManager actions)
    {

    }
}

