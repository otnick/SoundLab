using UnityEngine;
using System.Collections.Generic;
using SoundLab.Core;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace SoundLab.Sound{

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;

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

    public void OnActivate()
    {
        // Mute if already sustained
        GameController.Instance.AudioToChange.Add(audioSource);
            Debug.Log("Miau on");
    }

    public void OnDeactivate()
    {
        GameController.Instance.AudioToChange.Remove(audioSource);
            Debug.Log("miau de");

    }
    public void changeTargetVolume(float value)
    {
           Debug.Log("audiocontrollerObject parent name " + gameObject.name);
        Debug.Log(audioSource.volume + " changing it audio target: to " + value);
            //audioSource.volume = Mathf.Clamp(audioSource.volume + value, 0f, 1f);
            audioSource.volume = value;
    }
}
}