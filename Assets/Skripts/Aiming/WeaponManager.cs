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
    AudioSource audioSource;

    WPAmmo ammo;
    Bloom bloom;
    ActionStateManager actions;
    Recoil recoil;

    Light muzzleLight;
    ParticleSystem muzzleFlashParticals;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20;
    void Start()
    {
        recoil = GetComponent<Recoil>();
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<CameraAim>();
        ammo = GetComponent<WPAmmo>();
        bloom = GetComponent<Bloom>();
        actions = GetComponentInParent<ActionStateManager>();
        muzzleFlashParticals = GetComponentInChildren<ParticleSystem>();
        muzzleLight = GetComponentInChildren<Light>();
        lightIntensity = muzzleLight.intensity;
        muzzleLight.intensity = 0;
        fireRateTime = fireSpeed;
    }

    void Update()
    {
        if (ShouldFire()) Fire();
        muzzleLight.intensity = Mathf.Lerp(muzzleLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    bool ShouldFire() {
        fireRateTime += Time.deltaTime;
        if (fireRateTime < fireSpeed) return false;
        if (ammo.currentAmmo == 0) return false;
        if (actions.currentState==actions.Reload) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

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

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash() {
        muzzleFlashParticals.Play();
        muzzleLight.intensity = lightIntensity;
    }
}
