using UnityEngine;

public class InstrumentEQ : IEffects
{



    AudioLowPassFilter _lowpassFilter;
    AudioHighPassFilter _highpassFilter;

    public float scaleCoefficient = 100.0f; // this is used for setting the right cutoff freq

    public override void Init()
    {
        soundObject = this.gameObject;
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
        if (!_lowpassFilter.enabled && wetness > 0.9f)
        {
            _lowpassFilter.enabled = true;
            _highpassFilter.enabled = true;
        }
        if (wetness > 1.1f) {
            if (!_lowpassFilter.enabled)
            {
                _lowpassFilter.enabled = true;
                _highpassFilter.enabled = false;
            }
            float resonance = Mathf.Abs(Remap(wetness, 0.9f, 2f, 0f, 2f));
            wetness = Remap(wetness, 0.9f, 2f, 5000f, 400f);
            _lowpassFilter.cutoffFrequency = wetness;
            //_lowpassFilter.lowpassResonanceQ = 1 + resonance;

            Debug.Log("eq lowpass cutoff = " + _lowpassFilter.cutoffFrequency);

        } else
        {
            if (!_highpassFilter.enabled && wetness < 0.9f)
            {
                _lowpassFilter.enabled = false;
                _highpassFilter.enabled = true;
            }
            float resonance = Mathf.Abs(Remap(wetness, 1.1f, 0f, 0f, 5f));
            wetness = Remap(wetness, 1.1f, 0f, 0f, 1000f);
            _highpassFilter.cutoffFrequency = wetness;
            //_highpassFilter.highpassResonanceQ = 1 + resonance;

            Debug.Log("eq highpass cutoff = " + _highpassFilter.cutoffFrequency);
        }
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
