using UnityEngine;

public class ReverbEffect : IEffects
{
    private AudioReverbFilter _reverbFilter;

    // Settings for reverb
    public float scaleCoefficient = 1.0f; 
    public float reflectionsLevel = 100f;
    public float decayTime = 10.0f;
    public float dryLevel = 1.0f;

    public override void Init()
    {
        _reverbFilter = soundObject.GetComponent<AudioReverbFilter>();
        if (_reverbFilter == null)
        {
            _reverbFilter = soundObject.AddComponent<AudioReverbFilter>();
        }
        
        // Apply settings
        _reverbFilter.reverbPreset = AudioReverbPreset.User;
        _reverbFilter.decayTime = decayTime; 
        _reverbFilter.dryLevel = dryLevel;     // Keep dry signal full
        _reverbFilter.reflectionsLevel = reflectionsLevel;
        _reverbFilter.enabled = true;
    }

    public override void setWet(float wetness)
    {
        if (_reverbFilter != null)
        {
            // Normalize the scale: 0.5 scale = 0 (Dry), 1.5 scale = 1 (Full Wet)
            float normalizedWetness = Mathf.InverseLerp(0.5f, 1.5f, wetness);
            float finalWetness = Mathf.Lerp(0.2f, 1.0f, normalizedWetness);
            
            // Map 0.2-1.0 range to -1500 to 100 mB
            float reverbLevel = Mathf.Lerp(-1500f, 100f, finalWetness);
            _reverbFilter.reverbLevel = (int)reverbLevel;
            
            Debug.Log($"Reverb Scale: {wetness} | Normalized: {normalizedWetness} | Reverb: {reverbLevel} mB");
        }
    }
}
