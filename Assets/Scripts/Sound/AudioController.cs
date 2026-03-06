using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BendNote(float bendage)
    {
        audioSource.pitch = 1 + bendage;
    }
}
