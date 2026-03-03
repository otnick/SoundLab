using UnityEngine;

public class EQEffect : IEffects
{

    AudioLowPassFilter _lowpassFilter;

    public float scaleCoefficient = 100.0f; // this is used for setting the right cutoff freq

    public override void Init()
    {
        _lowpassFilter = soundObject.GetComponent<AudioLowPassFilter>();
        _lowpassFilter.enabled = true;
    }

    public override void setWet(float wetness)
    {
        _lowpassFilter.cutoffFrequency = wetness * scaleCoefficient;
        Debug.Log("eq cutoff = " + _lowpassFilter.cutoffFrequency);
    }
}
