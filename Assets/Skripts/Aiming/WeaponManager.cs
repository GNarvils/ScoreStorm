using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("Fire Rate Control")]
    [SerializeField] float fireSpeed;
    float fireRateTime;
    [SerializeField] bool semiAuto;

    [Header("Bullet")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerFire;
    public float damage = 20;
    CameraAim aim;
    [SerializeField] AudioClip gunShot;
    public AudioSource audioSource;

    public WPAmmo ammo;
    Bloom bloom;
    ActionStateManager actions;
    Recoil recoil;

    Light muzzleLight;
    ParticleSystem muzzleFlashParticals;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20;

    public float enemyKickBackForce = 100;

    public Transform leftHandTarget, leftHandHint;
    WeaponClassManager weaponClass;
    public GameTime gameTime;
    void Start()
    {
        aim = GetComponentInParent<CameraAim>();
        bloom = GetComponent<Bloom>();
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
    }

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
        if (ShouldFire()) Fire();
        muzzleLight.intensity = Mathf.Lerp(muzzleLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    //Funkcija, kas pārbauda vai šaut
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
    //Funkcija, kas šauj
    void Fire() {
        fireRateTime = 0;
        barrelPos.LookAt(aim.aimPos);
        barrelPos.localEulerAngles = bloom.bloomA(barrelPos);
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
    //Spēlē efektu, ka šauj
    void TriggerMuzzleFlash() {
        muzzleFlashParticals.Play();
        muzzleLight.intensity = lightIntensity;
    }
}
