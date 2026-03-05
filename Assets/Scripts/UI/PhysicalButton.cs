using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPress;
    [SerializeField] private float _pressDepth = 0.015f;

    private Vector3 _startPos;
    private bool    _isPressed;

    private void Start() => _startPos = transform.localPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (_isPressed) return;
        _isPressed = true;
        transform.localPosition = _startPos - new Vector3(0, _pressDepth, 0);
        _onPress.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _isPressed = false;
        transform.localPosition = _startPos;
    }
}