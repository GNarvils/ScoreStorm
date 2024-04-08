using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public float scrollDirection;
    public override void EnterState(ActionStateManager actions)
    {
        Debug.Log("Default");
    }

    public override void UpdateState(ActionStateManager actions)
    {
        actions.rHandAim.weight = Mathf.Lerp(actions.rHandAim.weight, 1, 10 * Time.deltaTime);
        actions.lHandIK.weight = Mathf.Lerp(actions.lHandIK.weight, 1, 10 * Time.deltaTime);

        if (!actions.health.isDead)
        {
            if (Input.GetKeyDown(KeyBinds.manager.reload) && CanReload(actions))
            {
                actions.SwitchState(actions.Reload);
            }
            else if (Input.GetKeyDown(KeyBinds.manager.swap))
            {
                actions.Default.scrollDirection = -1;
                actions.SwitchState(actions.Swap);
            }
            else if (Input.GetKeyDown(KeyBinds.manager.guard))
            {
                actions.SwitchState(actions.Guard);
            }
        }
    }

    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize) return false;
        else if (action.ammo.extraAmmo == 0) return false;
        else return true;
    }
}
