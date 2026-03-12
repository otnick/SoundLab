using UnityEngine;

public class MeshTracker : MonoBehaviour
{
    public GameObject _instrument;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       transform.position = _instrument.transform.position;
       transform.localScale = _instrument.transform.localScale;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _instrument.transform.position;
        transform.localScale = _instrument.transform.localScale;
    }
}
