using UnityEngine;
using SoundLab.Core;
using SoundLab.Sound;

public class PoseController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SustainInstrument(bool sustain)
    {
        GameController.Instance.Instrument.removeAudioSource(sustain);
        foreach (SoundTrigger sound in GameController.Instance.Sounds)
        {
            sound.Sustain(sustain);
        }
    }
    public void ResumeInstrument()
    {
        GameController.Instance.Instrument.addAudioSource();
    }

}
