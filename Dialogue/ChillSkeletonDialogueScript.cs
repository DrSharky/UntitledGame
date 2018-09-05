using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillSkeletonDialogueScript : MonoBehaviour
{

    public GameObject chillSkeleton;

    public Transform chillHead;
    public Transform newHead;
    public Transform neck;
    public Animation chillAnimation;
    public Transform playerCam;

    private bool lookAt = false;
	
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

		if(Input.GetKeyDown(KeyCode.E))
            StartDialogue();
	}

    void StartDialogue()
    {
        //TODO:
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            lookAt = false;
            chillHead.parent = neck;
            chillAnimation["Chill_Skelly"].AddMixingTransform(chillHead);
        }
    }
}