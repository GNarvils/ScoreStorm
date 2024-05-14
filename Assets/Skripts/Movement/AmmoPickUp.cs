using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour, IInteractable
{

    public WPAmmo weaponAmmo1;
    public WPAmmo weaponAmmo2;
    private Score playerScore;

    private void Start()
    {
        playerScore = FindObjectOfType<Score>();
    }
    //Kad pieskaras ar ammo box
    public void Interact()
    {
        if (weaponAmmo1 != null)
        {
            weaponAmmo1.extraAmmo += 15;
            weaponAmmo2.extraAmmo += 15;

            Debug.Log("Picked up ammo.");
            Destroy(gameObject);
            if (playerScore != null)
            {
                playerScore.AddToScore(25);
            }
        }
    }
}