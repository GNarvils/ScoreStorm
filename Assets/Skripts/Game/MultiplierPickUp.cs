using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierPickUp : MonoBehaviour, IInteractable
{
    public AudioClip clip;
    private AudioSource audioSource;
    private Renderer[] objectRenderers;
    private Collider[] objectColliders;

    private Score score;
    private bool isInteracted = false; //Vai ar objektu spēlētājam jau ir bijusi saskarne

    private void Start()
    {
        //Dabū vajdzīgās vērtības un komponentus
        int selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);
        GameObject selectedPlayerObject = null;

        if (selectedPlayer == 1)
        {
            selectedPlayerObject = GameObject.Find("Player_1");
        }
        else if (selectedPlayer == 2)
        {
            selectedPlayerObject = GameObject.Find("Player_2");
        }
        score = selectedPlayerObject.GetComponent<Score>();

        audioSource = GetComponent<AudioSource>();
        objectRenderers = GetComponentsInChildren<Renderer>();
        objectColliders = GetComponentsInChildren<Collider>();
        float soundVolume = PlayerPrefs.GetFloat("Sound");
        audioSource.volume = soundVolume;
    }
    //Saskarnes funkcija
    public void Interact()
    {
        //Ja score komponents ir atrasts un spēlētājam nav bijusi saskarne ar objektu izedara darbības
        if (!isInteracted && score != null)
        {
            isInteracted = true; //Spēlētājs ir saskāries
            score.ActivateScoreMultiplier(2f, 30f); //Aktivizē punktu reizinātāju
            audioSource.PlayOneShot(clip);//Spēle skaņu
            HideObject();//Paslēp objektu, ja uzreiz pazūd, bet ja skaņa var vel spēlēt
            StartCoroutine(DelayedDestroy()); //Iznīcina objektu pēc laika
        }
    }
    //Paslēp objektu
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
