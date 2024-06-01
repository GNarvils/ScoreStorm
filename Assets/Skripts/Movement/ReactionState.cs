using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        //Pārbauda vai pretinieks nebloķē
        if (actions.currentState != actions.Guard) { 
        //Roku svaru uzliek uz 0, lai varētu spēlēt rokas animāciju
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
