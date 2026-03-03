// ============================================================
//  Central singleton. Bootstraps all four systems.
// ============================================================
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

        public SceneController Scenes => _scenes;
        public VRController VR => _vr;
        public UIController UI => _ui;
        public TangibleController Tangible => _tangible;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _tangible.Connect();
        }

        private void OnDestroy()
        {
            _tangible.Disconnect();
        }
    }
}
