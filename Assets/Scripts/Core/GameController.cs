// controlls all other controllers
using UnityEngine;
using SoundLab.VR;
using SoundLab.UI;
using SoundLab.Tangible;
using System.Collections.Generic;
using SoundLab.Sound;

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

        

        public SceneController Scenes => _scenes;
        public VRController VR => _vr;
        public UIController UI => _ui;
        public TangibleController Tangible => _tangible;
        public SpawnController Spawn => _spawn;
        public AudioController Instrument => _instrument;
        public List<SoundTrigger> Sounds => _sounds;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            
        }

        private void OnDestroy()
        {
        }

        public void DebugHands()
        {
            Debug.Log("Triggered Fist Pose");
        }
        public void DebugHandsEnd()
        {
            Debug.Log("Finished fist pose");
            GameController.Instance.Instrument.enabled = true;
        }
    }
}
