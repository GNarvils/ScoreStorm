using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class WeaponClassManager : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint leftHandIk;
    public Transform recoilFollowPos;
    ActionStateManager actions;

    public void SetCurrentWeapon(WeaponManager weapon) {
    
    if(actions == null) actions = GetComponent<ActionStateManager>();
        leftHandIk.data.target = weapon.leftHandTarget;
        leftHandIk.data.hint = weapon.leftHandHint;
        actions.SetWeapon(weapon); 
    }
}
