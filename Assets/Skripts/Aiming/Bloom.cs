using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : MonoBehaviour
{
    [SerializeField] float defualtBloomAngle = 1f;
    [SerializeField] float walkBloomMultiplier = 2f;
    [SerializeField] float adsBloomMultiplier = 0f;

    CharacterMovement movement;
    CameraAim aiming;

    float currentBloom;
    void Start()
    {
        movement = GetComponentInParent<CharacterMovement>();
        aiming = GetComponentInParent<CameraAim>();
    }

    public Vector3 bloomA(Transform barrelPos) {
        if (movement.currentState == movement.idle) currentBloom = defualtBloomAngle;
        else if (movement.currentState == movement.walk) currentBloom = walkBloomMultiplier;

        if (aiming.currentState == aiming.Aim) currentBloom *= adsBloomMultiplier;

        float randX = Random.Range(-currentBloom, currentBloom);
        float randY = Random.Range(-currentBloom, currentBloom);
        float randZ = Random.Range(-currentBloom, currentBloom);
        Vector3 randomRotation = new Vector3(randX, randY, randZ);
        return barrelPos.localEulerAngles + randomRotation;
    }
}
