//  Async scene switching with a simple fade.
//  Setup: assign a full-screen black CanvasGroup as _fadeOverlay.
//  Scene build order: 0 = Title, 1 = Lab
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundLab.Core
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _fadeOverlay;
        [SerializeField] private float _fadeDuration = 0.4f;

        public void GoToTitle() => Load(0);
        public void GoToLab()   => Load(1);

        private void Load(int buildIndex, Action onDone = null)
        {
            StartCoroutine(LoadRoutine(buildIndex, onDone));
        }

        private IEnumerator LoadRoutine(int buildIndex, Action onDone)
        {
            yield return Fade(0f, 1f);

            var op = SceneManager.LoadSceneAsync(buildIndex);
            op.allowSceneActivation = false;
            while (op.progress < 0.9f) yield return null;
            op.allowSceneActivation = true;
            yield return null;

            yield return Fade(1f, 0f);
            onDone?.Invoke();
        }

        private IEnumerator Fade(float from, float to)
        {
            if (_fadeOverlay == null) yield break;
            _fadeOverlay.gameObject.SetActive(true);
            float t = 0f;
            while (t < _fadeDuration)
            {
                t += Time.deltaTime;
                _fadeOverlay.alpha = Mathf.Lerp(from, to, t / _fadeDuration);
                yield return null;
            }
            _fadeOverlay.alpha = to;
            if (to == 0f) _fadeOverlay.gameObject.SetActive(false);
        }
    }
}
