using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //Assigned in inspector.
    public GameObject playerGlasses;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            playerGlasses.SetActive(true);
            Destroy(gameObject);
        }
    }
}