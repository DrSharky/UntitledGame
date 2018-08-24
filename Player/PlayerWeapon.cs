using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public List<GameObject> weapons;

    private int currentWeapon;
    public string[] acquiredWeapons;

    private void Start()
    {
        acquiredWeapons = new string[3];
    }

    public void GiveWeapon(string weaponType)
    {

        switch (weaponType)
        {
            case "Leg":
                weapons[0].SetActive(true);
                currentWeapon = 0;
                acquiredWeapons[0] = weaponType;
                break;
            case "Fireball":
                weapons[1].SetActive(true);
                currentWeapon = 1;
                acquiredWeapons[1] = weaponType;
                break;
            case "Torch":
                weapons[2].SetActive(true);
                currentWeapon = 2;
                acquiredWeapons[2] = weaponType;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeapon == weapons.Count - 1)
                SetActiveWeapon(weapons[0]);
            else
                SetActiveWeapon(weapons[currentWeapon + 1]);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon == 0)
                SetActiveWeapon(weapons[weapons.Count - 1]);
            else
                SetActiveWeapon(weapons[currentWeapon - 1]);
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