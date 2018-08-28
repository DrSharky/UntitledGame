using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkeletonExplode : MonoBehaviour
{
    public GameObject LegWeaponPrefab;
    private UnityEvent explode;
    private MonsterAttack attackScript;
    private WanderingAI wanderScript;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        attackScript = transform.GetComponentInParent<MonsterAttack>();
        wanderScript = transform.GetComponentInParent<WanderingAI>();
        rb = GetComponent<Rigidbody>();
    }

    private void SeparateChildren(GameObject bone, List<GameObject> bones)
    {
        for(int i = 0; i < bone.transform.childCount; i++)
        {
            GameObject child = bone.transform.GetChild(i).gameObject;
            if (child.name == "upperLeg.L" || child.name == "upperLeg.R")
                break;
            child.GetComponent<Collider>().enabled = true;
            Rigidbody childRB = child.GetComponent<Rigidbody>();
            childRB.mass = 1.0f;
            childRB.angularDrag = 0.05f;
            childRB.useGravity = true;
            bones.Add(child);
            if (bone.name == "Spine3")
                continue;
            SeparateChildren(child, bones);
        }
        if (bone.name != "hips")
            bone.transform.DetachChildren();
        else
            bone.transform.GetChild(0).parent = null;
    }

    public void Boom()
    {
        explode = new UnityEvent();
        explode.AddListener(wanderScript.Boom);
        explode.AddListener(attackScript.Boom);
        explode.Invoke();

        List<GameObject> bones = new List<GameObject>();

        SeparateChildren(gameObject, bones);
        gameObject.GetComponent<Collider>().enabled = true;
        rb.mass = 1.0f;
        rb.angularDrag = 0.05f;
        rb.useGravity = true;
        rb.AddExplosionForce(10.0f, transform.position, 3.0f, 1.0f, ForceMode.Impulse);
        StartCoroutine(Cleanup(bones));
    }

    IEnumerator Cleanup(List<GameObject> bones)
    {
        yield return new WaitForSeconds(3.0f);
        foreach(GameObject bone in bones)
        {
            Destroy(bone);
        }

        GameObject skeleton = transform.parent.parent.gameObject;
        GameObject newWeapon = Instantiate(LegWeaponPrefab, transform.position + (new Vector3(0.0f, 0.5f, 0.0f)), transform.rotation);
        Destroy(skeleton);
    }
}