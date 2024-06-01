using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        //Roku svaru uzliek uz 0, lai varētu spēlēt rokas animāciju
        actions.rHandAim.weight = 0;
        actions.lHandIK.weight = 0;
        actions.anim.SetTrigger("Reload");
        Debug.Log("Reload");
    }

    public override void UpdateState(ActionStateManager actions)
    {

    }
}
