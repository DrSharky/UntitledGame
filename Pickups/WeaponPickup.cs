using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField]
    private string type;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            EventManager.TriggerEvent("GiveWeapon", type);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(transform.position.y < -200.0f)
            Destroy(gameObject);
    }
}