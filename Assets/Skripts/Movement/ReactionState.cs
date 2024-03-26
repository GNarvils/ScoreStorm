using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        if (actions.currentState != actions.Guard) { 
        actions.rHandAim.weight = 0;
        actions.lHandIK.weight = 0;
        actions.anim.SetTrigger("Reaction");
        Debug.Log("Reaction");
    }
    }

    public override void UpdateState(ActionStateManager actions)
    {

    }
}
