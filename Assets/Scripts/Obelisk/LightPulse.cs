using UnityEngine;

public class LightPulse : MonoBehaviour
{
    [SerializeField] private float _speed     = 1.5f;
    [SerializeField] private float _bottomY   = 0f;
    [SerializeField] private float _topY      = 2f;

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * _speed, 1f);
        var pos   = transform.localPosition;
        pos.y     = Mathf.Lerp(_bottomY, _topY, t);
        transform.localPosition = pos;
    }
}