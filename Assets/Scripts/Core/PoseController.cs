using UnityEngine;
using SoundLab.Core;
using SoundLab.Sound;

public class PoseController : MonoBehaviour
{
    public bool sustain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sustain = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SustainInstrument()
    {
        sustain = !sustain;
        //GameController.Instance.Instrument.removeAudioSource(sustain);
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
