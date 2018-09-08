using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillSkeletonDialogueScript : MonoBehaviour
{

    public GameObject chillSkeleton;
    public Transform chillHead;
    public Transform newHead;
    public Transform neck;
    public Transform rightHand;
    public Animation chillAnimation;
    public Transform playerCam;
    public GameObject speech;

    private UIManager ui;
    private bool lookAt = false;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
	{
        if (lookAt)
        {
            Vector3 targetDir = playerCam.position - newHead.position;

            float step = 1.0f * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(newHead.forward, targetDir, step, 0.0f);
            Debug.DrawRay(newHead.position, newDir, Color.red);
            newHead.rotation = Quaternion.LookRotation(newDir);
        }
        else
        {
            float step = 1.0f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(newHead.forward, newHead.forward, step, 0.0f);
            Debug.DrawRay(newHead.position, newDir, Color.green);
            newHead.rotation = Quaternion.LookRotation(newDir);
        }

        if (Input.GetKeyUp(KeyCode.E))
            StartDialogue();
	}

    void StartDialogue()
    {
        ui.DisplayDialoguePanel("ChillSkeleton");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            lookAt = true;
            //DOING THIS SO HEAD ANIMATIONS AND ROTATIONS COEXIST.
            chillHead.parent = newHead;
            chillHead.forward = -newHead.right;
            chillAnimation["Chill_Skelly"].RemoveMixingTransform(chillHead);
            speech.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            lookAt = false;
            chillHead.parent = neck;
            chillAnimation["Chill_Skelly"].AddMixingTransform(chillHead);
            chillAnimation["Chill_Skelly"].AddMixingTransform(rightHand);
            speech.SetActive(false);
        }
    }
}