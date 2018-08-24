using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPickup : MonoBehaviour
{
    public PlayerWeapon playerWeaponScript;
    public string weaponType;

    public void Start()
    {
        playerWeaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
    }
}