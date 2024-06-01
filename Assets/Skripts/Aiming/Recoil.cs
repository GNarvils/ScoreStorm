using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public Transform recoilFollowPos;
    [SerializeField] float kickBackAmount = -1; //Cik stipri kamera aizies atpakaļ
    [SerializeField] float kickBackSpeed = 10, returnSpeed = 20;//Iziešanas un atgriešanas ātrums
    float currentRecoilPos, finalRecoilPos; // Pašreizējā un gala atpakaļgaitas pozīcija

    void Update()
    {
        // Samazina pašreizējo atpakaļgaitas pozīciju, lai simulētu atgriešanos
        currentRecoilPos = Mathf.Lerp(currentRecoilPos, 0, returnSpeed * Time.deltaTime);
        // Veic lineāru interpolāciju starp pašreizējo un gala atpakaļgaitas pozīciju
        finalRecoilPos = Mathf.Lerp(finalRecoilPos, currentRecoilPos, kickBackSpeed * Time.deltaTime);
        // Atjauno recoilFollowPos transformāciju, lai simulētu atpakaļgaitu
        recoilFollowPos.localPosition = new Vector3(0, 0, finalRecoilPos);
    }

    //Metode, kas aizsit kameru uz atpkaļu, kad izšauj ieroci
    public void TriggerRecoil() { currentRecoilPos += kickBackAmount; }

}
