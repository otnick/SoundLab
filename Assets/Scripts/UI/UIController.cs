using System.Collections; // Required for Coroutines
using UnityEngine;
using UnityEngine.UI;
using SoundLab.Core;

namespace SoundLab.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private CanvasGroup _titleCanvasGroup; 
        [SerializeField] private float _fadeDuration = 1.5f;

        [Header("Environment to Wake Up")]
        [Tooltip("Drag systems here that should be activated when entering the lab (e.g. SunSystem)")]
        [SerializeField] private GameObject[] _environmentObjects;

        [Header("Title Buttons")]
        [SerializeField] private Button _enterLabBtn;
        [SerializeField] private Button _titleQuitBtn;

        private void Start()
        {
            if (_titleCanvasGroup != null)
            {
                _titleCanvasGroup.alpha = 1f;
                _titleCanvasGroup.interactable = true;
                _titleCanvasGroup.blocksRaycasts = true;
                _titleCanvasGroup.gameObject.SetActive(true);
            }

            foreach (GameObject obj in _environmentObjects)
            {
                if (obj != null) obj.SetActive(false);
            }

            if (_enterLabBtn) _enterLabBtn.onClick.AddListener(OnEnterLab);
            if (_titleQuitBtn) _titleQuitBtn.onClick.AddListener(OnQuit);
        }

        private void OnEnterLab()
        {
            if (_titleCanvasGroup != null)
            {
                // Start the fading process
                StartCoroutine(FocusShiftRoutine());
            }
        }

        private IEnumerator FocusShiftRoutine()
        {
            Debug.Log("Entering Lab: Starting Focus Shift fade...");

            _titleCanvasGroup.interactable = false;
            _titleCanvasGroup.blocksRaycasts = false;

            // 2. Smoothly fade the alpha from 1 to 0
            float timer = 0f;
            while (timer < _fadeDuration)
            {
                timer += Time.deltaTime; 
                _titleCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / _fadeDuration);
                yield return null; 
            }

            _titleCanvasGroup.alpha = 0f;

            foreach (GameObject obj in _environmentObjects)
            {
                if (obj != null) obj.SetActive(true);
            }

            _titleCanvasGroup.gameObject.SetActive(false);
            
            Debug.Log("Entered Lab: Environment Active, Title hidden.");
        }

        private void OnQuit()
        {
            Debug.Log("Quitting Application.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}