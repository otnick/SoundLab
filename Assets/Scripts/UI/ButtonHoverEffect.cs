using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoundLab.UI
{
    // These interfaces force the script to listen for the VR laser pointer
    public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Hover Settings")]
        [SerializeField] private Color _hoverColor = Color.cyan;
        [SerializeField] private float _scaleMultiplier = 1.1f;

        private Image _buttonImage;
        private Color _originalColor;
        private Vector3 _originalScale;

        private void Awake()
        {
            // Grab the image component and store the starting values
            _buttonImage = GetComponent<Image>();
            
            if (_buttonImage != null)
                _originalColor = _buttonImage.color;
                
            _originalScale = transform.localScale;
        }

        // Triggered the exact frame the laser touches the button
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_buttonImage != null)
                _buttonImage.color = _hoverColor;
                
            transform.localScale = _originalScale * _scaleMultiplier;
        }

        // Triggered the exact frame the laser leaves the button
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_buttonImage != null)
                _buttonImage.color = _originalColor;
                
            transform.localScale = _originalScale;
        }
    }
}