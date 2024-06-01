using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollManager : MonoBehaviour
{
    Rigidbody[] rbs;
    void Start()
    {
        //Dabū vajadzīgos komponentus
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs) rb.isKinematic = true;
    }

    //Aktivē modeļa ragdoll
    public void TriggerRagdoll() {
        foreach (Rigidbody rb in rbs) rb.isKinematic = false;
    }
}
