using UnityEngine;
using UnityEngine.UI;
using SoundLab.Core;

namespace SoundLab.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject _titleOverlayPanel;

        [Header("Title Buttons")]
        [SerializeField] public Button _enterLabBtn;
        [SerializeField] public Button _titleQuitBtn;

        private void Start()
        {
            // Force the overlay to be visible when the game starts
            if (_titleOverlayPanel) _titleOverlayPanel.SetActive(true);

            if (_enterLabBtn) _enterLabBtn.onClick.AddListener(OnEnterLab);
            if (_titleQuitBtn) _titleQuitBtn.onClick.AddListener(OnQuit);
        }

        // Hides the overlay to reveal the Lab behind it
        public void OnEnterLab()
        {
            if (_titleOverlayPanel)
            {
                _titleOverlayPanel.SetActive(false);
                Debug.Log("Entered Lab: Title Overlay hidden.");
            }
        }

        private void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}