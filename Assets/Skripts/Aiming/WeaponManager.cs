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
    CameraAim aim;
    [SerializeField] AudioClip gunShot;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<CameraAim>();
        fireRateTime = fireSpeed;
    }

    void Update()
    {
        if (ShouldFire()) Fire();
    }

    bool ShouldFire() {
        fireRateTime += Time.deltaTime;
        if (fireRateTime < fireSpeed) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire() {
        fireRateTime = 0;
        barrelPos.LookAt(aim.aimPos);
        audioSource.PlayOneShot(gunShot);
        for (int i = 0; i < bulletsPerFire; i++) {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }
}
