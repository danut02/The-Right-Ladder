using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThorns : MonoBehaviour
{
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Thorns" && MovementController.movementInstance.isSliding)
            Stamina.staminaInstance.grip -= 10f;
    }
}
