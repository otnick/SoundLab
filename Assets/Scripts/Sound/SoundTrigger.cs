using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using SoundLab.Core;

namespace SoundLab.Sound
{
    [RequireComponent(typeof(XRSimpleInteractable), typeof(AudioSource))]
    public class SoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private Color _defaultColor    = Color.white;
        [SerializeField] private Color _activeColor     = Color.cyan;
        [SerializeField] private float _hapticAmplitude = 0.5f;
        [SerializeField] private float _rotationSpeed   = 180f;
        [SerializeField] private float _fadeSpeed       = 8f;
        [SerializeField] private InputActionReference _sustainAction;
        [SerializeField] private ParticleSystem _particles;

        private AudioSource   _audio;
        private Renderer      _renderer;
        private Material      _material;
        private IXRInteractor _activeInteractor;
        private IXRInteractor _lastInteractor;
        private bool          _isSustained;
        private float         _targetVolume;

        private void Awake()
        {
            _audio    = GetComponent<AudioSource>();
            _renderer = GetComponentInChildren<Renderer>();
            _material = _renderer ? _renderer.material : null;

            _audio.clip = _clip;

            if (_material) _material.color = _defaultColor;

            var manager = LoopManager.Instance ?? FindFirstObjectByType<LoopManager>();
            if (manager != null)
                manager.Register(_audio);
            else
                Debug.LogError("[SoundTrigger] No LoopManager found in scene!", this);

            var interactable = GetComponent<XRSimpleInteractable>();
            interactable.selectEntered.AddListener(e => OnActivate(e.interactorObject));
            interactable.selectExited.AddListener(e  => OnDeactivate(e.interactorObject));
        }
        private void Start()
        {
            _isSustained = false;
            GameController.Instance.Sounds.Add(this);
        }

        private void Update()
        {
            _audio.volume = Mathf.Lerp(_audio.volume, _targetVolume, Time.deltaTime * _fadeSpeed);

            if (_activeInteractor is XRBaseInputInteractor inputInteractor)
            {
                inputInteractor.SendHapticImpulse(_hapticAmplitude, Time.deltaTime);
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            }
            else if (_isSustained)
            {
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            }
        }

        public void Sustain(bool sustain)
        {
            if (!sustain && _isSustained)
                Deactivate();
            else
                _isSustained = sustain;
        }


        private void OnActivate(IXRInteractor interactor)
        {
            // Mute if already sustained
            if (_isSustained && _targetVolume > 0)
            {
                _targetVolume = 0;
                if (_material) _material.color = _defaultColor;
                if (_particles) _particles.Stop();
                return;
            }

            {

            _activeInteractor = interactor;
            _lastInteractor = interactor;
            _targetVolume = 1f;
            _audio.volume = 1f; // set directly as well in case lerp is slow

            if (_material) _material.color = _activeColor;
            if (_particles) _particles.Play();
            }
        }

        private void OnDeactivate(IXRInteractor interactor)
        {
            _activeInteractor = null;
            //_isSustained      = IsPrimaryHeld();
            if (!_isSustained)
                Deactivate();
        }

        private void Deactivate()
        {
            _isSustained  = false;
            _targetVolume = 0f;
            _audio.volume = 0f;
            if (_material) _material.color = _defaultColor;
            if (_particles) _particles.Stop();
        }

    }
}
