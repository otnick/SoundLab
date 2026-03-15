using UnityEngine;
using SoundLab.Core;

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

    public void PauseInstrument()
    {
        GameController.Instance.Instrument.removeAudioSource();
    }
    public void ResumeInstrument()
    {
        GameController.Instance.Instrument.addAudioSource();
    }

}
