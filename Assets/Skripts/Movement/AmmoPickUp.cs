using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AmmoPickUp : MonoBehaviour, IInteractable
{
    public int ammoToAdd=10;
    public AudioClip clip;
    private AudioSource audioSource;
    private Renderer[] objectRenderers;
    private Collider[] objectColliders;
    private bool isInteracted = false;
    public WPAmmo weaponAmmo1;
    public WPAmmo weaponAmmo2;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        objectRenderers = GetComponentsInChildren<Renderer>();
        objectColliders = GetComponentsInChildren<Collider>();
        float soundVolume = PlayerPrefs.GetFloat("Sound");
        audioSource.volume = soundVolume;
    }

    public void Interact()
    {
        if (!isInteracted)
        {
            isInteracted = true;
            weaponAmmo1.extraAmmo += ammoToAdd;
            weaponAmmo2.extraAmmo += ammoToAdd;
            audioSource.PlayOneShot(clip);
            HideObject();
            StartCoroutine(DelayedDestroy());
        }
    }
    private void HideObject()
    {
        foreach (var renderer in objectRenderers)
        {
            renderer.enabled = false;
        }
        foreach (var collider in objectColliders)
        {
            collider.enabled = false;
        }
    }
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(clip.length);
        Destroy(gameObject);
    }
}