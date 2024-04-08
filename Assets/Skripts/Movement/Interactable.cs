using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable {
    public void Interact();
}
public class Interactable : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public KeyBinds key;
    void Start()
    {
        key = GetComponentInParent<KeyBinds>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyBinds.manager.interact)) {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                    interactObj.Interact();
                }
            }
        }
    }
}
