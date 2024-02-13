using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    public ActionBaseState currentState;

    public ReloadState Reload = new ReloadState();
    public DefaultState Default = new DefaultState();

    public GameObject currentWeapon;
    [HideInInspector] public WPAmmo ammo;
    AudioSource audioSource;
    public Animator anim;

    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;
    void Start()
    {
        SwitchState(Default);
        ammo = currentWeapon.GetComponent<WPAmmo>();
        audioSource = currentWeapon.GetComponent<AudioSource>();
    }
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void WeaponReloaded()
    {
        rHandAim.weight = 1;
        lHandIK.weight = 1;
        ammo.Reload();
        SwitchState(Default);
    }

    public void MagOut() {
        audioSource.PlayOneShot(ammo.magOutSound);
    }
    public void MagIn()
    {
        audioSource.PlayOneShot(ammo.magInSound);
    }
    public void ReloadSlide() {
        audioSource.PlayOneShot(ammo.slideSound);
    }
}
