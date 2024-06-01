using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    public ActionBaseState currentState; // Pašreizējais stāvoklis

    // Dažādi stāvokļi, kurus spēlētājs var ieņemt
    public ReloadState Reload = new ReloadState();
    public DefaultState Default = new DefaultState();
    public GuardState Guard = new GuardState();
    public ReactionState Reaction = new ReactionState();
    public DeathState Death = new DeathState();
    public SwapS Swap = new SwapS();

    public PlayerHealth health; 
    public KeyBinds key; 

    public WeaponManager currentWeapon; 
    [HideInInspector] public WPAmmo ammo; 
    AudioSource audioSource; 
    public Animator anim; 

    public MultiAimConstraint rHandAim; // Labās rokas mērķēšanas ierobežojums
    public TwoBoneIKConstraint lHandIK; // Kreisās rokas IK ierobežojums
    public GameTime gameTime;

    void Start()
    {
        // Dabū nepieciešamos komponentus
        key = GetComponentInParent<Transform>().parent.GetComponentInParent<KeyBinds>();
        health = GetComponentInParent<PlayerHealth>();
        SwitchState(Default); // Sākotnēji uzstāda spēlētāja stāvokli uz noklusēto
        GameObject uiGameObject = GameObject.Find("UI");
        if (uiGameObject != null)
        {
            gameTime = uiGameObject.GetComponent<GameTime>();
            if (gameTime == null)
            {
                Debug.LogError("GameTime skripts nav atrasts");
            }
        }
        else
        {
            Debug.LogError("UI GameObject nav atrasts.");
        }
    }

    void Update()
    {
        // Ja spēle ir beigusies, beidz stāvokļa maiņu
        if (gameTime.gameIsOver) return;

        // Atjauno pašreizējo stāvokli katrā kadru
        currentState.UpdateState(this);
    }

    // Metode, lai pārslēgtu spēlētāja stāvokli
    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    // Metode, kas tiek izsaukta, kad ierocis ir pārlādēts
    public void WeaponReloaded()
    {
        //Uzliek roku svaru uz 1
        rHandAim.weight = 1;
        lHandIK.weight = 1;
        ammo.Reload(); // Pārlādē ieroci
        SwitchState(Default); 
    }

    // Metode, kas tiek izsaukta, kad spēlētājs ir aizsargājies
    public void Guarded()
    {
        //Uzliek roku svaru uz 1
        rHandAim.weight = 
        lHandIK.weight = 1; 
        SwitchState(Default); 
    }

    // Metode, kas tiek izsaukta, kad spēlētājs ir reaģējis
    public void Reacted()
    {
        //Uzliek roku svaru uz 1
        rHandAim.weight = 1; 
        lHandIK.weight = 1;
        SwitchState(Default); 
    }

    // Metode, kas tiek izsaukta, kad izņem magazīnu
    public void MagOut()
    {
        audioSource.PlayOneShot(ammo.magOutSound); 
    }

    // Metode, kas tiek izsaukta, kad ieliek magazīnu
    public void MagIn()
    {
        audioSource.PlayOneShot(ammo.magInSound); 
    }

    // Metode, kas tiek izsaukta, kad pārlādē slīdi
    public void ReloadSlide()
    {
        audioSource.PlayOneShot(ammo.slideSound); 
    }

    // Metode, lai uzstādītu pašreizējo ieroci
    public void SetWeapon(WeaponManager weapon)
    {
        currentWeapon = weapon;
        audioSource = weapon.audioSource;
        ammo = weapon.ammo;
    }
}
