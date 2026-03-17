// controlls all other controllers
using UnityEngine;
using UnityEngine.SceneManagement;
using SoundLab.VR;
using SoundLab.UI;
using SoundLab.Tangible;
using System.Collections.Generic;
using System.Collections;
using SoundLab.Sound;
using SoundLab.Environment;

namespace SoundLab.Core
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [Header("Systems")]
        [SerializeField] private SceneController _scenes;
        [SerializeField] private VRController _vr;
        [SerializeField] private UIController _ui;
        [SerializeField] private TangibleController _tangible;
        [SerializeField] private SpawnController _spawn;
        [SerializeField] private AudioController _instrument;
        [SerializeField] private List<SoundTrigger> _sounds;
        [SerializeField] private SunriseController _sunrise;
        [SerializeField] private PoseController _pose;
        [SerializeField] private PressurePlateManager _pressurePlate;
        [SerializeField] private List<AudioSource> _audioToChange;

        

        public SceneController Scenes => _scenes;
        public VRController VR => _vr;
        public UIController UI => _ui;
        public TangibleController Tangible => _tangible;
        public SpawnController Spawn => _spawn;
        public AudioController Instrument => _instrument;
        public List<SoundTrigger> Sounds => _sounds;
        public SunriseController Sun => _sunrise;
        public PoseController Pose => _pose;
        public PressurePlateManager PressurePlate => _pressurePlate;
        public List<AudioSource> AudioToChange => _audioToChange;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene _, LoadSceneMode __)
        {
            _ui = FindObjectOfType<UIController>();
            _scenes = FindObjectOfType<SceneController>();
            _vr = FindObjectOfType<VRController>();
            _tangible = FindObjectOfType<TangibleController>();
            _spawn = FindObjectOfType<SpawnController>();
            _instrument = GameObject.Find("Instrument").GetComponent<AudioController>();
            _pose = FindObjectOfType<PoseController>();
            _pressurePlate = FindObjectOfType<PressurePlateManager>();

        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void StartExperience()
        {
            _ui.OnEnterLab();
            _scenes.GoToLab();
        }

        public void GoToTitle()
        {
            _ui.OnExitLab();
            _scenes.GoToTitle();
        }
    }
}
