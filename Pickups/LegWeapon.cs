using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LegWeapon : WeaponPickup
{
    private new void Start()
    {
        weaponType = "Leg";
        playerWeaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            playerWeaponScript.GiveWeapon(weaponType);
            Destroy(gameObject);
        }
    }
}