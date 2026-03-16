using SoundLab.Tangible;
using System.Collections;
//using System.Numerics;
using UnityEngine;

namespace SoundLab.Environment
{
    public class SunriseController : MonoBehaviour
    {
        [Header("Sun Objects")]
        [SerializeField] private Transform _sunRoot;   // rotates to lift the sun up
        [SerializeField] private Transform _sunPivot;  // moves sun closer as it rises
        [SerializeField] private Renderer _sunMesh;
        [SerializeField] private Renderer _corona1;
        [SerializeField] private Renderer _corona2;
        [SerializeField] private Renderer _corona3;
        [Header("God Rays")]
        [SerializeField] private ParticleSystem _godRays;
        [Header("Lighting")]
        [SerializeField] private Light _sunLight;
        [SerializeField] private float _maxLightIntensity = 1.2f;
        [Header("Skybox")]
        [SerializeField] private Material _skyboxMaterial;
        [Header("Sunrise Settings")]
        [SerializeField] private float _duration = 30f;
        [SerializeField] private float _startAngle = 0f;   // horizon
        [SerializeField] private float _endAngle = -90f;   // zenith
        [Header("Sun Distance")]
        [SerializeField] private float _startDistance = 60f;
        [SerializeField] private float _endDistance = 20f;
        [Header("Skybox Colors")]
        [SerializeField] private Color _skyNight = new Color(0.02f, 0.02f, 0.05f);
        [SerializeField] private Color _skySunrise = new Color(0.8f, 0.3f, 0.05f);
        [SerializeField] private Color _skyDay = new Color(0.9f, 0.6f, 0.2f);
        [SerializeField] private Color _groundNight = new Color(0.01f, 0.01f, 0.02f);
        [SerializeField] private Color _groundSunrise = new Color(0.15f, 0.05f, 0.01f);
        [SerializeField] private Color _groundDay = new Color(0.3f, 0.15f, 0.02f);

        private bool _isRunning = false;

        void Start()
        {
            //ResetTransforms();
            SetNight();

            // subscribe to websocket messages from the tangible controller
            if (Core.GameController.Instance?.Tangible != null)
                Core.GameController.Instance.Tangible.OnMessageReceived += HandleMessage;
            StartCoroutine(Sunrise());
        }

        void OnDestroy()
        {
            if (Core.GameController.Instance?.Tangible != null)
                Core.GameController.Instance.Tangible.OnMessageReceived -= HandleMessage;
        }

        // only care about "sunrise" type messages
        void HandleMessage(TangibleMessage msg)
        {
            if (msg.type != "sunrise") return;

            if (msg.action == "start") TriggerSunrise();
            else if (msg.action == "sunset") TriggerSunset();
            else if (msg.action == "reset") ResetToNight();
        }

        [ContextMenu("Trigger Sunrise")]
        public void TriggerSunrise()
        {
            if (_isRunning) return;
            StartCoroutine(SunriseRoutine());
        }

        [ContextMenu("Trigger Sunset")]
        public void TriggerSunset()
        {
            if (_isRunning) return;
            StartCoroutine(SunsetRoutine());
        }

        [ContextMenu("Reset to Night")]
        public void ResetToNight()
        {
            StopAllCoroutines();
            _isRunning = false;
            ResetTransforms();
            SetNight();
        }

        IEnumerator SunriseRoutine()
        {
            _isRunning = true;
            float elapsed = 0f;

            if (_godRays != null) _godRays.Play();

            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _duration);

                // rotate sun upward
                if (_sunRoot != null)
                    _sunRoot.rotation = Quaternion.Euler(Mathf.Lerp(_startAngle, _endAngle, EaseInOut(t)), 0f, 0f);

                // move sun closer
                if (_sunPivot != null)
                    _sunPivot.localPosition = new Vector3(0f, 0f, Mathf.Lerp(_startDistance, _endDistance, EaseInOut(t)));

                UpdateSkybox(t);
                if (_sunLight != null) _sunLight.intensity = Mathf.Lerp(0f, _maxLightIntensity, EaseInOut(t));
                UpdateCorona(t);

                yield return null;
            }

            _isRunning = false;
        }

        IEnumerator SunsetRoutine()
        {
            _isRunning = true;
            float elapsed = 0f;

            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _duration);

                // reverse — sun goes back down
                if (_sunRoot != null)
                    _sunRoot.rotation = Quaternion.Euler(Mathf.Lerp(_endAngle, _startAngle, EaseInOut(t)), 0f, 0f);

                if (_sunPivot != null)
                    _sunPivot.localPosition = new Vector3(0f, 0f, Mathf.Lerp(_endDistance, _startDistance, EaseInOut(t)));
           

                UpdateSkybox(1f - t);
                if (_sunLight != null) _sunLight.intensity = Mathf.Lerp(_maxLightIntensity, 0f, EaseInOut(t));
                UpdateCorona(1f - t);

                yield return null;
            }

            if (_godRays != null) _godRays.Stop();
            SetNight();
            _isRunning = false;
        }

        void ResetTransforms()
        {
            if (_sunRoot != null)
                _sunRoot.rotation = Quaternion.Euler(_startAngle, 0f, 0f);
            if (_sunPivot != null)
                _sunPivot.localPosition = new Vector3(0f, 0f, _startDistance);
        }

        void SetNight()
        {
            UpdateSkybox(0f);
            if (_sunLight != null) _sunLight.intensity = 0f;
            UpdateCorona(0f);
            if (_godRays != null) _godRays.Stop();
        }

        void UpdateSkybox(float t)
        {
            if (_skyboxMaterial == null) return;

            // two-phase blend: night -> sunrise -> day
            Color sky = t < 0.5f
                ? Color.Lerp(_skyNight, _skySunrise, t * 2f)
                : Color.Lerp(_skySunrise, _skyDay, (t - 0.5f) * 2f);

            Color ground = t < 0.5f
                ? Color.Lerp(_groundNight, _groundSunrise, t * 2f)
                : Color.Lerp(_groundSunrise, _groundDay, (t - 0.5f) * 2f);

            _skyboxMaterial.SetColor("_SkyTint", sky);
            _skyboxMaterial.SetColor("_GroundColor", ground);
            _skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(0.3f, 1.2f, t));
        }

        void UpdateCorona(float t)
        {
            if (_sunMesh != null)
                _sunMesh.material.SetColor("_EmissionColor", new Color(1f, 0.9f, 0.7f) * Mathf.Lerp(2f, 8f, t));

            // fade in corona rings
            SetCoronaAlpha(_corona1, Mathf.Lerp(0f, 0.6f, t));
            SetCoronaAlpha(_corona2, Mathf.Lerp(0f, 0.4f, t));
            SetCoronaAlpha(_corona3, Mathf.Lerp(0f, 0.2f, t));
        }

        void SetCoronaAlpha(Renderer r, float alpha)
        {
            if (r == null) return;
            var c = r.material.color;
            c.a = alpha;
            r.material.color = c;
        }

        // smooth ease in/out curve
        float EaseInOut(float t) => t * t * (3f - 2f * t);
        private IEnumerator Sunrise()
        {
            // Start function WaitAndPrint as a coroutine
            yield return new WaitForSeconds(5.0f);
            TriggerSunrise();
            StartCoroutine(Sunset());
        }

        private IEnumerator Sunset()
        {

            // Start function WaitAndPrint as a coroutine
            yield return new WaitForSeconds(110.0f);
            TriggerSunset();
        }
    }
   
}
