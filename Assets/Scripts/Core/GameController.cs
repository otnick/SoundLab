// controlls all other controllers
using UnityEngine;
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

        

        public SceneController Scenes => _scenes;
        public VRController VR => _vr;
        public UIController UI => _ui;
        public TangibleController Tangible => _tangible;
        public SpawnController Spawn => _spawn;
        public AudioController Instrument => _instrument;
        public List<SoundTrigger> Sounds => _sounds;
        public SunriseController Sun => _sunrise;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartCoroutine(Sunrise());
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
        private IEnumerator Sunrise()
        {
            print("Starting " + Time.time);

            // Start function WaitAndPrint as a coroutine
            yield return new WaitForSeconds(5.0f);
            Sun.TriggerSunrise();
            StartCoroutine(Sunset());
        }

        private IEnumerator Sunset()
        {
            print("Starting " + Time.time);

            // Start function WaitAndPrint as a coroutine
            yield return new WaitForSeconds(20.0f);
            Sun.TriggerSunset();
        }
    }
}
