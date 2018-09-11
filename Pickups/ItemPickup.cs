using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemPickup : MonoBehaviour
{
    //Assigned in inspector.

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            EventManager.TriggerEvent("Sunglasses");
            Destroy(gameObject);
        }
    }
}