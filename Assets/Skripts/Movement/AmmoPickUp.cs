using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour, IInteractable
{

    public WPAmmo weaponAmmo;


    public void Interact()
    {
        if (weaponAmmo != null)
        {
            weaponAmmo.extraAmmo += 15;

            Debug.Log("Picked up 15 extra ammo.");
            gameObject.SetActive(false);
        }
    }
}