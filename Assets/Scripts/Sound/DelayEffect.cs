using UnityEngine;

public class DelayEffect : IEffects
{
    private AudioEchoFilter _echoFilter;

    // Settings for delay
    public float delayTime = 500f;
    public float decayRatio = 0.5f;
    public float wetMix = 0f;

    public override void Init()
    {
        // It is called AudioEchoFilter in unity
        _echoFilter = soundObject.GetComponent<AudioEchoFilter>();
        if (_echoFilter == null)
        {
            _echoFilter = soundObject.AddComponent<AudioEchoFilter>();
        }

        // Apply settings
        _echoFilter.delay = delayTime;
        _echoFilter.decayRatio = decayRatio;
        _echoFilter.wetMix = wetMix;
        _echoFilter.enabled = true;
    }

    public override void setWet(float wetness)
    {
        if (_echoFilter != null)
        {
            // Normalize the scale: 0.5 (Base) to 1.5 (Max)
            float normalizedWetness = Mathf.InverseLerp(0.5f, 1.5f, wetness);
            
            // Mapping for the "Wet Mix" (volume of the echoes)
            float perceivedWetness = Mathf.Sqrt(normalizedWetness);
            
            // Map 0-1 perceived wetness to 0-1 wet mix
            _echoFilter.wetMix = perceivedWetness;

            // Increase the delay time slightly as you scale
            _echoFilter.delay = Mathf.Lerp(300f, 800f, perceivedWetness);

            Debug.Log($"Delay Scale: {wetness} | Wet Mix: {_echoFilter.wetMix:F2} | Delay: {_echoFilter.delay:F0}ms");
        }
    }
}
