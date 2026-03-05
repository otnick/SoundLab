using UnityEngine;
using UnityEngine.UI;
using SoundLab.Core;

namespace SoundLab.UI
{
    public class SpawnPanel : MonoBehaviour
    {
        [Header("Pages")]
        [SerializeField] private GameObject _soundSelectPage;
        [SerializeField] private GameObject _effectSelectPage;

        [Header("Effect Toggle Buttons")]
        [SerializeField] private Button _reverbBtn;
        [SerializeField] private Button _delayBtn;
        [SerializeField] private Button _eqBtn;

        [Header("Colors")]
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _pressedColor = Color.green;

        private int  _selectedSound;
        private bool _reverb, _delay, _eq;

        private void Start() => ShowSoundSelect();

        public void SelectSound1() => AdvanceToEffects(0);
        public void SelectSound2() => AdvanceToEffects(1);

        public void ToggleReverb() { _reverb = !_reverb; UpdateButtonColor(_reverbBtn, _reverb); }
        public void ToggleDelay()  { _delay  = !_delay;  UpdateButtonColor(_delayBtn,  _delay); }
        public void ToggleEQ()     { _eq     = !_eq;     UpdateButtonColor(_eqBtn,     _eq); }

        public void OnSpawn()
        {
            GameController.Instance.Spawn.Spawn(_selectedSound, _reverb, _delay, _eq);
            ShowSoundSelect();
        }

        public void OnBack() => ShowSoundSelect();

        private void AdvanceToEffects(int soundIndex)
        {
            _selectedSound = soundIndex;
            _soundSelectPage.SetActive(false);
            _effectSelectPage.SetActive(true);
        }

        private void UpdateButtonColor(Button btn, bool active)
        {
            if (btn != null) btn.targetGraphic.color = active ? _pressedColor : _defaultColor;
        }

        private void ShowSoundSelect()
        {
            _selectedSound = 0;
            _reverb = _delay = _eq = false;
            _soundSelectPage.SetActive(true);
            _effectSelectPage.SetActive(false);
            UpdateButtonColor(_reverbBtn, false);
            UpdateButtonColor(_delayBtn,  false);
            UpdateButtonColor(_eqBtn,     false);
        }
    }
}
