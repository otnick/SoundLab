using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace SoundLab.Sound
{
    [RequireComponent(typeof(XRSimpleInteractable), typeof(AudioSource))]
    public class SoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private Color _defaultColor  = Color.white;
        [SerializeField] private Color _activeColor   = Color.cyan;
        [SerializeField] private float _hapticAmplitude = 0.5f;
        [SerializeField] private float _rotationSpeed   = 180f;

        private AudioSource _audio;
        private Renderer    _renderer;
        private IXRInteractor _activeInteractor;
        private bool        _isSustained;

        private void Awake()
        {
            _audio    = GetComponent<AudioSource>();
            _renderer = GetComponentInChildren<Renderer>();

            _audio.playOnAwake = false;
            _audio.loop        = true;

            if (_renderer) _renderer.material.color = _defaultColor;

            var interactable = GetComponent<XRSimpleInteractable>();
            interactable.selectEntered.AddListener(e => OnActivate(e.interactorObject));
            interactable.selectExited.AddListener(e  => OnDeactivate(e.interactorObject));

            // Primary button toggle for sustain
            interactable.activated.AddListener(_ => ToggleSustain());
        }

        private void Update()
        {
            if (_activeInteractor is XRBaseInputInteractor inputInteractor || _isSustained)
            {
                if (_activeInteractor is XRBaseInputInteractor activeInput)
                {
                    activeInput.SendHapticImpulse(_hapticAmplitude, Time.deltaTime);
                }
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            }
        }

        private void ToggleSustain()
        {
            Debug.Log("ToggleSustain called. Current state: " + _isSustained);
            _isSustained = !_isSustained;
            
            if (_isSustained)
            {
                if (!_audio.isPlaying)
                {
                    _audio.clip = _clip;
                    _audio.Play();
                }
                if (_renderer) _renderer.material.color = _activeColor;
            }
            else if (_activeInteractor == null)
            {
                _audio.Stop();
                if (_renderer) _renderer.material.color = _defaultColor;
            }
        }

        private void OnActivate(IXRInteractor interactor)
        {
            _activeInteractor = interactor;
            if (!_audio.isPlaying)
            {
                _audio.clip = _clip;
                _audio.Play();
            }
            if (_renderer) _renderer.material.color = _activeColor;
        }

        private void OnDeactivate(IXRInteractor interactor)
        {
            _activeInteractor = null;
            if (!_isSustained)
            {
                _audio.Stop();
                if (_renderer) _renderer.material.color = _defaultColor;
            }
        }
    }
}
