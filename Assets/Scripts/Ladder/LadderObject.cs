using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MovementController;
using TMPro;

public class LadderObject : MonoBehaviour, ILadderObjectInteractable
{
    public bool Interact(LadderObjectInteractor interactor)
    {
        Monologue.instance.DisplayText("Obiectul a fost distrus!",3);
        // here we can do anything when we press E
        GameObject myObject = GameObject.Find("LadderInteractableObject");
        if (myObject != null) {
            Destroy(myObject);
        }
        return true;
    }
}