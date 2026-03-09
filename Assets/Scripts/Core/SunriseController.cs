// ============================================================
//  SunriseController.cs
// ============================================================

using System.Collections;
using UnityEngine;
using SoundLab.Tangible;

namespace SoundLab.Environment
{
    public class SunriseController : MonoBehaviour
    {
        [Header("Sun Objects")]
        [SerializeField] private Transform       _sunRoot;
        [SerializeField] private Renderer        _sunMesh;
        [SerializeField] private Renderer        _corona1;
        [SerializeField] private Renderer        _corona2;
        [SerializeField] private Renderer        _corona3;

        [Header("God Rays")]
        [SerializeField] private ParticleSystem  _godRays;

        [Header("Lighting")]
        [SerializeField] private Light           _sunLight;
        [SerializeField] private float           _maxLightIntensity = 1.2f;

        [Header("Skybox")]
        [SerializeField] private Material        _skyboxMaterial;

        [Header("Sunrise Settings")]
        [SerializeField] private float           _duration   = 30f;
        [SerializeField] private float           _startAngle = 200f;
        [SerializeField] private float           _endAngle   = 140f;

        [Header("Skybox Colors")]
        [SerializeField] private Color _skyNight      = new Color(0.02f, 0.02f, 0.05f);
        [SerializeField] private Color _skySunrise    = new Color(0.8f,  0.3f,  0.05f);
        [SerializeField] private Color _skyDay        = new Color(0.9f,  0.6f,  0.2f);
        [SerializeField] private Color _groundNight   = new Color(0.01f, 0.01f, 0.02f);
        [SerializeField] private Color _groundSunrise = new Color(0.15f, 0.05f, 0.01f);
        [SerializeField] private Color _groundDay     = new Color(0.3f,  0.15f, 0.02f);

        private bool _isRunning = false;

        private void Start()
        {
            if (_sunRoot != null)
                _sunRoot.rotation = Quaternion.Euler(_startAngle, 0f, 0f);

            SetNight();

            if (Core.GameController.Instance?.Tangible != null)
                Core.GameController.Instance.Tangible.OnMessageReceived += HandleMessage;
        }

        private void OnDestroy()
        {
            if (Core.GameController.Instance?.Tangible != null)
                Core.GameController.Instance.Tangible.OnMessageReceived -= HandleMessage;
        }

        private void HandleMessage(TangibleMessage msg)
        {
            if (msg.type != "sunrise") return;
            switch (msg.action)
            {
                case "start": TriggerSunrise(); break;
                case "reset": ResetToNight();   break;
            }
        }

        [ContextMenu("Trigger Sunrise")]
        public void TriggerSunrise()
        {
            if (_isRunning) return;
            StartCoroutine(SunriseRoutine());
        }

        [ContextMenu("Reset to Night")]
        public void ResetToNight()
        {
            StopAllCoroutines();
            _isRunning = false;
            if (_sunRoot != null)
                _sunRoot.rotation = Quaternion.Euler(_startAngle, 0f, 0f);
            SetNight();
        }

        private IEnumerator SunriseRoutine()
        {
            _isRunning = true;
            float elapsed = 0f;

            if (_godRays != null) _godRays.Play();

            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _duration);

                if (_sunRoot != null)
                {
                    float angle = Mathf.Lerp(_startAngle, _endAngle, EaseInOut(t));
                    _sunRoot.rotation = Quaternion.Euler(angle, 0f, 0f);
                }

                UpdateSkybox(t);

                if (_sunLight != null)
                    _sunLight.intensity = Mathf.Lerp(0f, _maxLightIntensity, EaseInOut(t));

                UpdateCorona(t);

                yield return null;
            }

            _isRunning = false;
        }

        private void SetNight()
        {
            UpdateSkybox(0f);
            if (_sunLight != null) _sunLight.intensity = 0f;
            UpdateCorona(0f);
            if (_godRays != null) _godRays.Stop();
        }

        private void UpdateSkybox(float t)
        {
            if (_skyboxMaterial == null) return;

            Color sky = t < 0.5f
                ? Color.Lerp(_skyNight,    _skySunrise, t * 2f)
                : Color.Lerp(_skySunrise,  _skyDay,    (t - 0.5f) * 2f);

            Color ground = t < 0.5f
                ? Color.Lerp(_groundNight,   _groundSunrise, t * 2f)
                : Color.Lerp(_groundSunrise, _groundDay,    (t - 0.5f) * 2f);

            _skyboxMaterial.SetColor("_SkyTint",     sky);
            _skyboxMaterial.SetColor("_GroundColor", ground);
            _skyboxMaterial.SetFloat("_Exposure",    Mathf.Lerp(0.3f, 1.2f, t));
        }

        private void UpdateCorona(float t)
        {
            if (_sunMesh != null)
                _sunMesh.material.SetColor("_EmissionColor",
                    new Color(1f, 0.9f, 0.7f) * Mathf.Lerp(2f, 8f, t));

            SetCoronaAlpha(_corona1, Mathf.Lerp(0f, 0.6f, t));
            SetCoronaAlpha(_corona2, Mathf.Lerp(0f, 0.4f, t));
            SetCoronaAlpha(_corona3, Mathf.Lerp(0f, 0.2f, t));
        }

        private void SetCoronaAlpha(Renderer r, float alpha)
        {
            if (r == null) return;
            var c = r.material.color;
            c.a = alpha;
            r.material.color = c;
        }

        private float EaseInOut(float t) => t * t * (3f - 2f * t);
    }
}