using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        Debug.Log("AudioManager Start method says hi");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BendNote(float bendage)
    {
        Debug.Log("bending note");
        audioSource.pitch = 1 + bendage;
    }
}
