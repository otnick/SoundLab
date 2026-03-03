using System;
using UnityEngine;


namespace SoundLab.VR
{
    public class VRController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor _left;
        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor _right;

        // Other systems subscribe to these instead of XR events directly maybe? Could be useful for morph and stuff later
        public event Action<GameObject> OnGrabbed;
        public event Action<GameObject> OnReleased;
        public event Action<GameObject> OnHoverEntered;
        public event Action<GameObject> OnHoverExited;

        // Raw controller positions -> used by morph later
        public Vector3 LeftHandPos  => _left.transform.position;
        public Vector3 RightHandPos => _right.transform.position;

        private void OnEnable()
        {
            _left.selectEntered.AddListener(e  => OnGrabbed?.Invoke(e.interactableObject.transform.gameObject));
            _left.selectExited.AddListener(e   => OnReleased?.Invoke(e.interactableObject.transform.gameObject));
            _left.hoverEntered.AddListener(e   => OnHoverEntered?.Invoke(e.interactableObject.transform.gameObject));
            _left.hoverExited.AddListener(e    => OnHoverExited?.Invoke(e.interactableObject.transform.gameObject));

            _right.selectEntered.AddListener(e => OnGrabbed?.Invoke(e.interactableObject.transform.gameObject));
            _right.selectExited.AddListener(e  => OnReleased?.Invoke(e.interactableObject.transform.gameObject));
            _right.hoverEntered.AddListener(e  => OnHoverEntered?.Invoke(e.interactableObject.transform.gameObject));
            _right.hoverExited.AddListener(e   => OnHoverExited?.Invoke(e.interactableObject.transform.gameObject));
        }

        private void OnDisable()
        {
            _left.selectEntered.RemoveAllListeners();
            _left.selectExited.RemoveAllListeners();
            _left.hoverEntered.RemoveAllListeners();
            _left.hoverExited.RemoveAllListeners();

            _right.selectEntered.RemoveAllListeners();
            _right.selectExited.RemoveAllListeners();
            _right.hoverEntered.RemoveAllListeners();
            _right.hoverExited.RemoveAllListeners();
        }
    }
}
