using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{
    float top = 0.05f;
    float bottom = -0.021f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPressurePlateHeight(float value)
    {
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            Remap(Mathf.Clamp(value, 0f, 1.3f), 0f, 1.3f, bottom, top),
            transform.localPosition.z
            );
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
