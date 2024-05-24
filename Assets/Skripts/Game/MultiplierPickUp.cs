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
    private bool isInteracted = false;

    private void Start()
    {
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

    public void Interact()
    {
        if (!isInteracted && score != null)
        {
            isInteracted = true;
            score.ActivateScoreMultiplier(2f, 30f);
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
