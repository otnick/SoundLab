// controlls all other controllers
using UnityEngine;
using SoundLab.VR;
using SoundLab.UI;
using SoundLab.Tangible;

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
        [SerializeField] private SunriseController _sunrise;

        public SceneController Scenes => _scenes;
        public VRController VR => _vr;
        public UIController UI => _ui;
        public TangibleController Tangible => _tangible;
        public SpawnController Spawn => _spawn;
        public SunriseController Sunrise => _sunrise;

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
    }
}
