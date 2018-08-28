using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private PlayerWeapon playerWeaponScript;
    [SerializeField]
    private GameObject playerWeapon;
    

    private void Start()
    {
        playerWeaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            playerWeaponScript.GiveWeapon(playerWeapon);
            Destroy(gameObject);
        }
    }
}