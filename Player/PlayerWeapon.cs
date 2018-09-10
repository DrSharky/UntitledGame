using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{

    //DEV VAR REMOVE ON BUILD
    public bool debug = true;
    //DEV VAR REMOVE ON BUILD

    public List<GameObject> weapons;
    public List<GameObject> acquiredWeapons;

    public GameObject sunglasses;

    private int currentWeapon;
    private Animator currentAnim;
    private float shootCooldown = 1.0f;
    private bool cooldownWait = false;
    private bool swingCooldownWait = false;
    private float swingCooldown = 0.1f;
    [SerializeField]
    private GameObject fireballProjectile;
    private bool mouseButtonUp = true;
    private bool gotSunglasses = false;

    private void Start()
    {
        acquiredWeapons = new List<GameObject>();
        if (debug)
        {
            currentWeapon = 0;
            acquiredWeapons.Add(GameObject.FindGameObjectWithTag("Fireball"));
        }
        else
            currentWeapon = -1;
    }

    public void GiveWeapon(GameObject weapon)
    {
        GameObject weaponInList = weapons.Where(x => x.name == weapon.name).FirstOrDefault();
        if (weaponInList != null)
        {
            if (acquiredWeapons.Where(x => x.name == weapon.name).FirstOrDefault() == null)
            {
                acquiredWeapons.Add(weaponInList);
                SetActiveWeapon(weaponInList);
            }
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
        if (Input.GetMouseButton(0) && currentWeapon > -1 && mouseButtonUp)
        {
            mouseButtonUp = false;
            Attack();
        }
        if (Input.GetMouseButtonUp(0))
            mouseButtonUp = true;

        if (Input.GetKeyDown(KeyCode.Q) && UIManager.sunglasses)
        {
            if (sunglasses.activeInHierarchy)
                sunglasses.SetActive(false);
            else
                sunglasses.SetActive(true);
        }
        if (UIManager.initSun)
        {
            sunglasses.SetActive(true);
            UIManager.initSun = false;
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
        GameObject camera = transform.GetChild(0).gameObject;

        if (acquiredWeapons[currentWeapon].name == "Fireball" && !cooldownWait)
        {
            GameObject fireball = Instantiate(fireballProjectile, camera.transform.position + (camera.transform.forward), Quaternion.identity);
            acquiredWeapons[currentWeapon].SetActive(false);
            fireball.GetComponent<Rigidbody>().velocity = camera.transform.forward * 10;
            fireball.transform.right = -transform.forward;
            StartCoroutine(StartCooldown());
        }
        else if (currentAnim != null && !swingCooldownWait)
        {
            currentAnim.Play("Attack");
            
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 2.65f))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.yellow, 20.0f);
                SkullAttack skullScript = hit.transform.gameObject.GetComponent<SkullAttack>();
                if (skullScript != null)
                    skullScript.TakeDamage();
            }
            StartCoroutine(SwingCooldown());
        }
        else
            return;
    }

    IEnumerator StartCooldown()
    {
        cooldownWait = true;
        yield return new WaitForSeconds(shootCooldown);
        cooldownWait = false;
        acquiredWeapons[currentWeapon].SetActive(true);
    }

    IEnumerator SwingCooldown()
    {
        swingCooldownWait = true;
        yield return new WaitForSeconds(swingCooldown);
        swingCooldownWait = false;
    }
}