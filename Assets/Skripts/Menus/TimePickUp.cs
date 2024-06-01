using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TimePickUp : MonoBehaviour, IInteractable
{
    public int addTime = 30; //Cik laiku pieliks klāt
    public AudioClip clip;
    private AudioSource audioSource;
    private Renderer[] objectRenderers;
    private Collider[] objectColliders;

    private GameTime gameTime;
    private bool isInteracted = false;

    private void Start()
    {
        //Atrod vajadzīgos komponentus

        GameObject uiObject = GameObject.Find("UI");

        gameTime = uiObject.GetComponent<GameTime>();

        audioSource = GetComponent<AudioSource>();
        objectRenderers = GetComponentsInChildren<Renderer>();
        objectColliders = GetComponentsInChildren<Collider>();
        float soundVolume = PlayerPrefs.GetFloat("Sound");
        audioSource.volume = soundVolume;
    }

    //Kad lietotājam ir saskarne
    public void Interact()
    {
        //Ja spēlētājs nav saskaries ar objektu un gameTime komponents eksistē, tad izdara darbību
        if (!isInteracted && gameTime != null)
        {
            isInteracted = true; //Pataisa vērtību par true, lai spēlētājs nevarētu saskarties vairākas reizes
            gameTime.AddTime(addTime); //Pieskata klāt laiku
            audioSource.PlayOneShot(clip); //Spēlē skaņas klipu
            HideObject();//Paslēp objektu
            StartCoroutine(DelayedDestroy()); //Iznīcina objektu
        }
    }
    //Paslēp objektu, lai skaņu varētu izpildīties
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


    //Iznīcina objektu, kad skaņas klips ir beidzies
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(clip.length);
        Destroy(gameObject);
    }
}
