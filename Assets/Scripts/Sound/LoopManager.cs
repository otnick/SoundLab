using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the looping of audio sources for the unmuting system


namespace SoundLab.Sound
{
    public class LoopManager : MonoBehaviour
    {
        public static LoopManager Instance { get; private set; }

        [SerializeField] private double _startDelay = 0.5;

        private readonly List<AudioSource> _sources = new();

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private IEnumerator Start()
        {
            yield return null; // wait one frame so all SoundTriggers have registered
            double startTime = AudioSettings.dspTime + _startDelay;
            foreach (var source in _sources)
                source.PlayScheduled(startTime);
        }

        public void Register(AudioSource source)
        {
            source.loop        = true;
            source.volume      = 0f;
            source.playOnAwake = false;
            _sources.Add(source);
        }
    }
}
