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
        [SerializeField] private Color _defaultColor  = Color.white;
        [SerializeField] private Color _activeColor   = Color.cyan;
        [SerializeField] private float _hapticAmplitude = 0.5f;
        [SerializeField] private float _rotationSpeed   = 180f;
        [SerializeField] private InputActionReference _sustainAction;

        private AudioSource   _audio;
        private Renderer      _renderer;
        private Material      _material;
        private IXRInteractor _activeInteractor;
        private IXRInteractor _lastInteractor;
        private bool          _isSustained;

        private void Awake()
        {
            _audio    = GetComponent<AudioSource>();
            _renderer = GetComponentInChildren<Renderer>();
            _material = _renderer ? _renderer.material : null;

            _audio.playOnAwake = false;
            _audio.loop        = true;

            if (_material) _material.color = _defaultColor;

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
            if (_activeInteractor is XRBaseInputInteractor inputInteractor)
            {
                inputInteractor.SendHapticImpulse(_hapticAmplitude, Time.deltaTime);
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            }
            else if (_isSustained)
            {
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
            }
            else if (!_isSustained) StopSound();
        }

        private bool IsPrimaryHeld()
        {
            return _sustainAction != null && _sustainAction.action.IsPressed();
        }

        public void Sustain(bool sustain)
        {
            
            _isSustained = sustain;
            Debug.Log("sustain is " + _isSustained);
        }

        private void StopSound()
        {
            //_isSustained = false;
            _audio.Stop();
            if (_material) _material.color = _defaultColor;
        }

        private void OnActivate(IXRInteractor interactor)
        {
            _activeInteractor = interactor;
            _lastInteractor   = interactor;
            _audio.clip = _clip;
            _audio.Play();
            if (_material) _material.color = _activeColor;
        }

        private void OnDeactivate(IXRInteractor interactor)
        {
            _activeInteractor = null;
            //_isSustained      = IsPrimaryHeld();
            if (!_isSustained)
                StopSound();
        }
    }
}
