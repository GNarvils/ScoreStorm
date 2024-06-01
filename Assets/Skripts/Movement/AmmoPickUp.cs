using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AmmoPickUp : MonoBehaviour, IInteractable
{
    public int ammoToAdd=10; //Cik lodes pielikt klāt
    public AudioClip clip;
    private AudioSource audioSource;
    private Renderer[] objectRenderers;
    private Collider[] objectColliders;
    private bool isInteracted = false; //Vai ir bijusi saksarne
    public WPAmmo weaponAmmo1; //Ierocis 1
    public WPAmmo weaponAmmo2; //Ierocis 2

    private void Start()
    {
        //Dabū vajadzīgos komponentus un vērtības
        audioSource = GetComponent<AudioSource>();
        objectRenderers = GetComponentsInChildren<Renderer>();
        objectColliders = GetComponentsInChildren<Collider>();
        float soundVolume = PlayerPrefs.GetFloat("Sound");
        audioSource.volume = soundVolume;
    }

    //Ja ir saskarne
    public void Interact()
    {
        //Ja spēlētājs nav saskāries 
        if (!isInteracted)
        {
            isInteracted = true; // Spēlētājs ir saskāries
            weaponAmmo1.extraAmmo += ammoToAdd; //Pieliek ierocim 1 lodes klāt
            weaponAmmo2.extraAmmo += ammoToAdd; //Pieliek ierocim 2 lodes klāt
            audioSource.PlayOneShot(clip); // Spēle skaņu
            HideObject(); //Paslēpj objektu, ja varētu vel spēlēt skaņu
            StartCoroutine(DelayedDestroy()); //Pēc skaņas iznīcina
        }
    }
    //Paslēpj objektu, ja nevar to redzēt
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
    //Iznīcina objektu pēc skaņas klipa beigšanas
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(clip.length);
        Destroy(gameObject);
    }
}