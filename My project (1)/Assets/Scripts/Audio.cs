using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip clip;

    public void AudioAssign()
    {
        audio= GetComponent<AudioSource>();
    }

    public void AudioPlay(Collider other)
    {
        audio.Play();
    }

}
