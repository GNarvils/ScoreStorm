using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    public ActionBaseState currentState;

    public ReloadState Reload = new ReloadState();
    public DefaultState Default = new DefaultState();
    public GuardState Guard = new GuardState();
    public ReactionState Reaction = new ReactionState();
    public DeathState Death = new DeathState();
    public SwapS Swap = new SwapS();
    public PlayerHealth health;

    public WeaponManager currentWeapon;
    [HideInInspector] public WPAmmo ammo;
    AudioSource audioSource;
    public Animator anim;

    public MultiAimConstraint rHandAim;
    public TwoBoneIKConstraint lHandIK;
    void Start()
    {
        health = GetComponentInParent<PlayerHealth>();
        SwitchState(Default);
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

    public void Guarded() {
        rHandAim.weight = 1;
        lHandIK.weight = 1;
        SwitchState(Default);
    }

    public void Reacted()
    {
        rHandAim.weight = 1;
        lHandIK.weight = 1;
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
    public void SetWeapon(WeaponManager weapon) {
        currentWeapon = weapon;
        audioSource = weapon.audioSource;
        ammo = weapon.ammo;
    }
}
