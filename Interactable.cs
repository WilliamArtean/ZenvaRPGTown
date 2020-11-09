using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    [SerializeField]
    private string interactDescription;
    public UnityEvent onInteract;

    public void Interact() {
        if (onInteract != null) {
            onInteract.Invoke();
        }
    }

    public string GetInteractDescription() {
        return interactDescription;
    }

}
