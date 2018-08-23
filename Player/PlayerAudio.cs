using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip damage1;
    public AudioClip damage2;
    public AudioClip death;
    public AudioSource audioDamage;

    public AudioClip walk1;
    public AudioClip walk2;
    public AudioSource audioWalk;

    private Coroutine playWalk = null;
    private Coroutine playRun = null;
    private Rigidbody rb;
    private RigidbodyFirstPersonController moveScript;
    private float moveSpeed;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<RigidbodyFirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = moveScript.movementSettings.ForwardSpeed;

        if (rb.velocity.magnitude > 2.0f && rb.velocity.magnitude < 10.0f && audioWalk.isPlaying == false)
        {
            if(playRun != null)
                StopCoroutine(playRun);
            playWalk = StartCoroutine(PlayWalk());
        }
        else
        {
            if (rb.velocity.magnitude > 10f && audioWalk.isPlaying == false)
            {
                if(playWalk != null)
                    StopCoroutine(playWalk);
                audioWalk.pitch = 1.5f;
                playRun = StartCoroutine(PlayRun());
            }
            if(rb.velocity.magnitude < 2f)
            {
                if(playWalk != null)
                    StopCoroutine(playWalk);
                if(playRun != null)
                    StopCoroutine(playRun);
            }
        }
    }

    public IEnumerator PlayDamage()
    {
        audioDamage.Play();

        yield return new WaitForSeconds(audioDamage.clip.length);

        if (audioDamage.clip == damage1)
            audioDamage.clip = damage2;
        else
            audioDamage.clip = damage1;
    }

    public IEnumerator PlayDeath()
    {
        audioDamage.clip = death;
        audioDamage.Play();
        yield return new WaitForSeconds(audioDamage.clip.length + 3.0f);
    }

    public IEnumerator PlayWalk()
    {
        audioWalk.Play();

        yield return new WaitForSeconds(0.4f);

        if (audioWalk.clip == walk1)
            audioWalk.clip = walk2;
        else
            audioWalk.clip = walk1;
    }

    public IEnumerator PlayRun()
    {
        audioWalk.Play();
        yield return new WaitForSeconds(0.25f);
        if (audioWalk.clip == walk1)
            audioWalk.clip = walk2;
        else
            audioWalk.clip = walk1;

    }
}