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
        // Normalize the scale: 0.5 scale = 0 (Dry), 1.5 scale = 1 (Full Wet)
        // for eq we do small object -> high freq
        //float normalizedWetness = Mathf.Lerp(0.3f, 4f, wetness);
        
        // Map 0-1 normalized wetness to 200z to 22000Hz (audible range)
        //_lowpassFilter.cutoffFrequency = Mathf.Lerp(200f, 22000f, wetness);

        _lowpassFilter.cutoffFrequency = wetness * scaleCoefficient;
        
        Debug.Log("eq cutoff = " + _lowpassFilter.cutoffFrequency);
    }
}
