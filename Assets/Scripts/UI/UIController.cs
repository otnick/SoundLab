using UnityEngine;
using UnityEngine.UI;
using SoundLab.Core;

namespace SoundLab.UI
{
    public class UIController : MonoBehaviour
    {
        // Title Scene 
        [Header("Title")]
        [SerializeField] private Button _enterLabBtn;
        [SerializeField] private Button _quitBtn;

        //Lab Scene
        [Header("Lab")]
        [SerializeField] private Button _backToTitleBtn;

        //Spawn Panel
        [Header("Spawn Panel")]
        [SerializeField] private GameObject _spawnPanel;

        private void Start()
        {
            if (_enterLabBtn)  _enterLabBtn.onClick.AddListener(OnEnterLab);
            if (_quitBtn)      _quitBtn.onClick.AddListener(OnQuit);
            if (_backToTitleBtn) _backToTitleBtn.onClick.AddListener(OnBackToTitle);
            
            if (_spawnPanel) _spawnPanel.SetActive(true);
        }

        // switches scenes
        private void OnEnterLab()    => GameController.Instance.Scenes.GoToLab();
        private void OnBackToTitle() => GameController.Instance.Scenes.GoToTitle();

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
