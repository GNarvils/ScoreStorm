using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WPAmmo : MonoBehaviour
{
    public int clipSize;
    public int extraAmmo;
    public int currentAmmo;

    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip slideSound;
    public TMP_Text mag; 
    public TMP_Text ammoLeft;
    void Start()
    {
        currentAmmo = clipSize;
    }

    private void Update()
    {
        //Ammo Teksts
        mag.text = currentAmmo.ToString();
        ammoLeft.text = extraAmmo.ToString();
    }


    //Reload funkcija
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
