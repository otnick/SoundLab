using UnityEngine;

public class InstrumentEQ : IEffects
{



    AudioLowPassFilter _lowpassFilter;
    AudioHighPassFilter _highpassFilter;

    public float scaleCoefficient = 100.0f; // this is used for setting the right cutoff freq

    public override void Init()
    {
        _lowpassFilter = soundObject.GetComponent<AudioLowPassFilter>();
        _lowpassFilter.enabled = true;
        _highpassFilter = soundObject.GetComponent<AudioHighPassFilter>();
        _highpassFilter.enabled = false;
    }

    public override void setWet(float wetness)
    {
        // Normalize the scale: 0.5 scale = 0 (Dry), 1.5 scale = 1 (Full Wet)
        // for eq we do small object -> high freq
        //float normalizedWetness = Mathf.Lerp(0.3f, 4f, wetness);

        // Map 0-1 normalized wetness to 200z to 22000Hz (audible range)
        //_lowpassFilter.cutoffFrequency = Mathf.Lerp(200f, 22000f, wetness);

        // lowpass
        if (wetness > 0) {
            if (!_lowpassFilter.enabled)
            {
                _lowpassFilter.enabled = true;
                _highpassFilter.enabled = false;
            }

            _lowpassFilter.cutoffFrequency = 1000 - wetness * scaleCoefficient;

            Debug.Log("eq lowpass cutoff = " + _lowpassFilter.cutoffFrequency);

        } else
        {
            if (!_highpassFilter.enabled)
            {
                _lowpassFilter.enabled = false;
                _highpassFilter.enabled = true;
            }
            _highpassFilter.cutoffFrequency = wetness * scaleCoefficient * -2;

            Debug.Log("eq highpass cutoff = " + _highpassFilter.cutoffFrequency);
        }
    }

}
