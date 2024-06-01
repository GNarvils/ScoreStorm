using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WPAmmo : MonoBehaviour
{
    public int clipSize; // Magazīnas lielums
    public int extraAmmo; // Pārlikušās lodes
    public int currentAmmo; // Tagadējošas lodes daudzums magazīna.

    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip slideSound;
    public TMP_Text ammoText;

    void Start()
    {
        //Tagadējošas lodes ir tikpat daudz, kā citi magazīnā.
        currentAmmo = clipSize;
    }

    private void Update()
    {
        //Parāda lodes aktuālās
        ammoText.text = currentAmmo.ToString() + " / " + extraAmmo.ToString();
    }

    //Pārlādēšanas funkcija
    public void Reload()
    {
        if (extraAmmo >= clipSize)
        {
            int ammoToReload = clipSize - currentAmmo;
            extraAmmo -= ammoToReload;
            currentAmmo += ammoToReload;
        }
        else if (extraAmmo > 0)
        {
            if (extraAmmo + currentAmmo > clipSize)
            {
                int leftOverAmmo = extraAmmo + currentAmmo - clipSize;
                extraAmmo = leftOverAmmo;
                currentAmmo = clipSize;
            }
            else
            {
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
        }
    }
}
