using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("Šaušanas kontrole")]
    [SerializeField] float fireSpeed; // Ieroča šaušanas ātrums
    float fireRateTime; // Laiks starp divām šāvieniem
    [SerializeField] bool semiAuto; // Vai ierocis ir semiAuto režīmā

    [Header("Lode")]
    [SerializeField] GameObject bullet; //Lodes objekts
    [SerializeField] Transform barrelPos; //No kurienes lode tiks izšauta
    [SerializeField] float bulletVelocity; // Lodes ātrums
    [SerializeField] int bulletsPerFire; // Cik lodes tiek izšautas šāvienā
    public float damage = 20; // Cik daudz dzīvības atņems pretiniekam.
    CameraAim aim;
    [SerializeField] AudioClip gunShot;
    public AudioSource audioSource;

    public WPAmmo ammo;
    ActionStateManager actions;
    Recoil recoil;

    Light muzzleLight;
    ParticleSystem muzzleFlashParticals;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20;

    public float enemyKickBackForce = 100; //Ar kādu stiprumu iešaus pretiniekam.

    public Transform leftHandTarget, leftHandHint; //Kreisās rokas lokācija
    WeaponClassManager weaponClass;
    public GameTime gameTime;
    void Start()
    {
        //Dabū vajadzīgos komponentus
        aim = GetComponentInParent<CameraAim>();
        actions = GetComponentInParent<ActionStateManager>();
        muzzleFlashParticals = GetComponentInChildren<ParticleSystem>();
        muzzleLight = GetComponentInChildren<Light>();
        lightIntensity = muzzleLight.intensity;
        muzzleLight.intensity = 0;
        fireRateTime = fireSpeed;

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

         float soundVolume = PlayerPrefs.GetFloat("Sound");
         audioSource.volume = soundVolume;
    }
    // Kad objekts tiek aktivizēts, iestata pašreizējo ieroci un munīciju
    private void OnEnable()
    {
        if (weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            recoil = GetComponent<Recoil>();
            audioSource = GetComponent<AudioSource>();
            ammo = GetComponent<WPAmmo>();
        }
        weaponClass.SetCurrentWeapon(this);
    }

    void Update()
    {
        // Ja drīkst šaut, tad šauj
        if (ShouldFire()) Fire();
        // Atgriež gaismas intensitāti uz sākotnējo vērtību
        muzzleLight.intensity = Mathf.Lerp(muzzleLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    //Funkcija, kas pārbauda vai tagad drīkst šauj un vai nav stāvoklī, kurā nevajadzētu tagad būt.
    bool ShouldFire() {
        fireRateTime += Time.deltaTime;
        if (fireRateTime < fireSpeed) return false;
        if (ammo.currentAmmo == 0) return false;
        if (actions.currentState == actions.Reload) return false;
        if (actions.currentState == actions.Swap) return false;
        if (actions.currentState == actions.Guard) return false;
        if (actions.currentState == actions.Reaction) return false;
        if (actions.currentState == actions.Death) return false;
        if (aim.currentState == aim.Hip) return false;
        if (gameTime.gameIsOver) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }
    //Funkcija, kas šauj ieroci
    void Fire() {
        fireRateTime = 0;
        barrelPos.LookAt(aim.aimPos);
        audioSource.PlayOneShot(gunShot);
        recoil.TriggerRecoil();
        TriggerMuzzleFlash();
        ammo.currentAmmo--;
        for (int i = 0; i < bulletsPerFire; i++) {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            
            Bullet bulletScirpt = currentBullet.GetComponent<Bullet>();
            bulletScirpt.weapon = this;

            bulletScirpt.dir = barrelPos.transform.forward;

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }
    //Spēlē efektu, ka šauj IEROCI
    void TriggerMuzzleFlash() {
        muzzleFlashParticals.Play();
        muzzleLight.intensity = lightIntensity;
    }
}
