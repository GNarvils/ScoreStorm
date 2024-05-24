﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TimePickUp : MonoBehaviour, IInteractable
{
    public int addTime = 30;
    public AudioClip clip;
    private AudioSource audioSource;
    private Renderer[] objectRenderers;
    private Collider[] objectColliders;

    private GameTime gameTime;
    private bool isInteracted = false;

    private void Start()
    {

        GameObject uiObject = GameObject.Find("UI");

        gameTime = uiObject.GetComponent<GameTime>();

        audioSource = GetComponent<AudioSource>();
        objectRenderers = GetComponentsInChildren<Renderer>();
        objectColliders = GetComponentsInChildren<Collider>();
        float soundVolume = PlayerPrefs.GetFloat("Sound");
        audioSource.volume = soundVolume;
    }

    public void Interact()
    {
        if (!isInteracted && gameTime != null)
        {
            isInteracted = true;
            gameTime.AddTime(addTime);
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
