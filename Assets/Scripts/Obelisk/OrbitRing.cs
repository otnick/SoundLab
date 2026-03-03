using UnityEngine;

public class OrbitRing : MonoBehaviour
{
    [SerializeField] private float _speed  = 30f;   // Grad pro Sekunde
    [SerializeField] private Vector3 _axis = Vector3.up;

    private void Update()
    {
        transform.Rotate(_axis, _speed * Time.deltaTime);
    }
}