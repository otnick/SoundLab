using UnityEngine;

namespace SoundLab.Core
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private GameObject  _soundInstancePrefab;
        [SerializeField] private AudioClip[] _soundClips;           // 0 = Sound1, 1 = Sound2
        [SerializeField] private float       _spawnDistance = 1.5f;

        private Transform _head;

        private void Start() => _head = Camera.main.transform;

        public void Spawn(int soundIndex, bool reverb, bool delay, bool eq)
        {
            Vector3 pos = _head.position + _head.forward * _spawnDistance;
            GameObject root = Instantiate(_soundInstancePrefab, pos, Quaternion.identity);

            AudioSource audio = root.GetComponentInChildren<AudioSource>(true);
            if (audio != null && soundIndex < _soundClips.Length)
            {
                audio.clip = _soundClips[soundIndex];
                audio.Play();
            }

            SetEffectActive<ReverbEffect>(root, reverb);
            SetEffectActive<DelayEffect>(root, delay);
            SetEffectActive<EQEffect>(root, eq);
        }

        private void SetEffectActive<T>(GameObject root, bool active) where T : IEffects
        {
            T effect = root.GetComponentInChildren<T>(true);
            if (effect != null) effect.gameObject.SetActive(active);
        }
    }
}
