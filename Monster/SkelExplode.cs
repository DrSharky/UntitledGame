using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkelExplode : MonoBehaviour
{
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

    public void Boom()
    {
        explode = new UnityEvent();
        explode.AddListener(wanderScript.Boom);
        explode.AddListener(attackScript.Boom);
        explode.Invoke();

        GameObject[] bones = new GameObject[7];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.transform.position = transform.position;
            Rigidbody childRB = child.GetComponent<Rigidbody>();
            childRB.mass = 1.0f;
            childRB.angularDrag = 0.05f;
            childRB.useGravity = true;
            bones[i] = child;
        }
        rb.mass = 1.0f;
        rb.angularDrag = 0.05f;
        rb.useGravity = true;

        GameObject spine1 = transform.parent.parent.gameObject;
        GameObject legL = spine1.transform.parent.GetChild(0).gameObject;
        GameObject legR = spine1.transform.parent.GetChild(1).gameObject;
        Rigidbody legLRB = legL.GetComponent<Rigidbody>();
        Rigidbody legRRB = legR.GetComponent<Rigidbody>();
        Rigidbody spine1RB = transform.parent.parent.GetComponent<Rigidbody>();
        spine1RB.mass = 1.0f;
        spine1RB.angularDrag = 0.05f;
        spine1RB.useGravity = true;
        bones[3] = spine1;
        legLRB.mass = 1.0f;
        legLRB.angularDrag = 0.05f;
        legLRB.useGravity = true;
        bones[4] = legL;
        legRRB.mass = 1.0f;
        legRRB.angularDrag = 0.05f;
        legRRB.useGravity = true;
        bones[5] = legR;
        Rigidbody spine2RB = transform.parent.gameObject.GetComponent<Rigidbody>();
        spine2RB.mass = 1.0f;
        spine2RB.angularDrag = 0.05f;
        spine2RB.useGravity = true;
        bones[6] = transform.parent.gameObject;
        transform.parent.DetachChildren();
        gameObject.transform.DetachChildren();
        spine1.transform.DetachChildren();
        spine1.transform.parent.DetachChildren();
        StartCoroutine(Cleanup(bones));
    }

    IEnumerator Cleanup(GameObject[] bones)
    {
        yield return new WaitForSeconds(3.0f);
        foreach(GameObject bone in bones)
        {
            Destroy(bone);
        }
        Destroy(gameObject);
    }
}