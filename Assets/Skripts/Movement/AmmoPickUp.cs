using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour, IInteractable
{

    public WPAmmo weaponAmmo;
    private Score playerScore;

    private void Start()
    {
        playerScore = FindObjectOfType<Score>();
    }
    //Kad pieskaras ar ammo box
    public void Interact()
    {
        if (weaponAmmo != null)
        {
            weaponAmmo.extraAmmo += 10;

            Debug.Log("Picked up 10 extra ammo.");
            Destroy(gameObject);
            if (playerScore != null)
            {
                playerScore.AddToScore(25);
            }
        }
    }
}