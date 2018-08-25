using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public List<GameObject> weapons;

    private int currentWeapon;
    private List<GameObject> acquiredWeapons;

    private void Start()
    {
        acquiredWeapons = new List<GameObject>();
    }

    public void GiveWeapon(GameObject weapon)
    {
        if (weapons[weapons.IndexOf(weapon)] != null)
        {
            weapons[weapons.IndexOf(weapon)].SetActive(true);
            currentWeapon = weapons.IndexOf(weapon);
            acquiredWeapons.Add(weapon);
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (acquiredWeapons.Count <= 1)
                return;

            if (currentWeapon == acquiredWeapons.Count - 1)
                SetActiveWeapon(acquiredWeapons[0]);
            else
                SetActiveWeapon(acquiredWeapons[currentWeapon + 1]);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (acquiredWeapons.Count <= 1)
                return;

            if (currentWeapon == 0)
                SetActiveWeapon(acquiredWeapons[acquiredWeapons.Count - 1]);
            else
                SetActiveWeapon(acquiredWeapons[currentWeapon - 1]);
        }
    }

    private void SetActiveWeapon(GameObject weapon)
    {
        for(int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }
        weapon.SetActive(true);
        currentWeapon = weapons.IndexOf(weapon);
    }
}
