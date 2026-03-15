using UnityEngine;
using System.Collections.Generic;
using SoundLab.Core;

namespace SoundLab.Sound{

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removeAudioSource(bool pause)
        {
            if (pause) audioSource.Pause();
            else audioSource.Play();
        }
    public void addAudioSource()
    {
            audioSource.Play();
    }

    public void BendNote(float bendage)
    {
        audioSource.pitch = 1 + bendage;
    }
}
}