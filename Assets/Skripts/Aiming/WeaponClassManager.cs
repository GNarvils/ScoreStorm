using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponClassManager : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint leftHandIk; // IK komponente kreisajai rokai
    public Transform recoilFollowPos; // Transformācija, lai sekotu recoil pozīcijai
    ActionStateManager actions; 

    public WeaponManager[] weapons; // Masīvs ar visiem ieročiem
    int currentWeaponIndex; // Pašreizējā ieroča indekss

    private void Awake()
    {
        currentWeaponIndex = 0; // Uzstāda sākotnējo ieroča indeksu uz 0 (pirmais ierocis)

        // Dabū ieročus, aktivizējot tikai pirmo ieroci
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == 0) weapons[i].gameObject.SetActive(true);
            else weapons[i].gameObject.SetActive(false);
        }
    }

    // Metode, lai uzstādītu pašreizējo ieroci
    public void SetCurrentWeapon(WeaponManager weapon)
    {
        if (actions == null)
            actions = GetComponent<ActionStateManager>(); // Ja nav iestatīts, iegūst ActionStateManager komponenti

        // Uzstāda IK mērķi un norādi kreisajai rokai uz ieroča vērtībām
        leftHandIk.data.target = weapon.leftHandTarget;
        leftHandIk.data.hint = weapon.leftHandHint;

        actions.SetWeapon(weapon); // Uzstāda ieroci ActionStateManager
    }

    // Metode, lai nomainītu ieroci
    public void ChangeWeapon(float direction)
    {
        // Deaktivizē pašreizējo ieroci
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        // Maina ieroča indeksu atkarībā no virziena
        if (direction < 0)
        {
            if (currentWeaponIndex == 0) currentWeaponIndex = weapons.Length - 1;
            else currentWeaponIndex--;
        }
        else
        {
            if (currentWeaponIndex == weapons.Length - 1) currentWeaponIndex = 0;
            else currentWeaponIndex++;
        }

        // Aktivizē jauno ieroci
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    // Metode, kas tiek izsaukta, kad ierocis tiek nolikts malā
    public void WeaponPutAway()
    {
        ChangeWeapon(actions.Default.scrollDirection);
    }

    // Metode, kas tiek izsaukta, kad ierocis tiek izvilkts
    public void WeaponPulledOut()
    {
        actions.SwitchState(actions.Default); // Pārslēdz stāvokli uz noklusēto
    }
}