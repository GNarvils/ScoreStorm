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
        //Pārbauda vai roku svars ir pareizais
        actions.rHandAim.weight = Mathf.Lerp(actions.rHandAim.weight, 1, 10 * Time.deltaTime);
        actions.lHandIK.weight = Mathf.Lerp(actions.lHandIK.weight, 1, 10 * Time.deltaTime);
        //Pārbauda vai spēlētājs nav miris
        if (!actions.health.isDead)
        {
            //Ja nospiež pārlādēšanas pogu un spēlētājs var pārlādēt, tad pārlādē ieroci
            if (Input.GetKeyDown(KeyBinds.manager.reload) && CanReload(actions))
            {
                actions.SwitchState(actions.Reload);
            }
            //Ja nospiež ieroča maiņu pogu, tad spēlētājs maina ieroci
            else if (Input.GetKeyDown(KeyBinds.manager.swap))
            {
                actions.Default.scrollDirection = -1;
                actions.SwitchState(actions.Swap);
            }
            //Ja nospiež bloķēšans pogu, tad spēlētājs bloķē
            else if (Input.GetKeyDown(KeyBinds.manager.guard))
            {
                actions.SwitchState(actions.Guard);
            }
        }
    }
    //Pārbauda vai spēlētājs var pārlādēt ieroci
    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize) return false;
        else if (action.ammo.extraAmmo == 0) return false;
        else return true;
    }
}
