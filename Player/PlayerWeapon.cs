using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{
    public List<GameObject> weapons;
    public List<GameObject> acquiredWeapons;

    private int currentWeapon;
    private Animator currentAnim;
    private float shootCooldown = 1.0f;
    private bool cooldownWait = false;
    [SerializeField]
    private GameObject fireballProjectile;
    private List<GameObject> acquiredWeapons;

    private void Start()
    {
        acquiredWeapons = new List<GameObject>();
    }

    public void GiveWeapon(GameObject weapon)
    {
        GameObject weaponInList = weapons.Where(x => x.name == weapon.name).FirstOrDefault();
        if (weaponInList != null)
        {
            acquiredWeapons.Add(weaponInList);
            SetActiveWeapon(weaponInList);
        }
        else
        {
            Debug.LogWarning("Weapon hasn't been added to possible pickup list! Check the weapon names.");
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
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    private void SetActiveWeapon(GameObject weapon)
    {
        for(int i = 0; i < acquiredWeapons.Count; i++)
        {
            acquiredWeapons[i].SetActive(false);
        }
        weapon.SetActive(true);
        currentWeapon = acquiredWeapons.IndexOf(weapon);
        currentAnim = acquiredWeapons[currentWeapon].GetComponent<Animator>();
    }

    private void Attack()
    {
        if (currentAnim != null)
            currentAnim.Play("Attack");

        if(acquiredWeapons[currentWeapon].name == "Fireball" && !cooldownWait)
        {
            GameObject camera = transform.GetChild(0).gameObject;
            GameObject fireball = Instantiate(fireballProjectile, camera.transform.position + (camera.transform.forward), Quaternion.identity);
            fireball.GetComponent<Rigidbody>().velocity = camera.transform.forward * 10;
            fireball.transform.right = -transform.forward;
            cooldownWait = true;
            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator StartCooldown()
    {
        cooldownWait = true;
        yield return new WaitForSeconds(shootCooldown);
        cooldownWait = false;
    }
}
