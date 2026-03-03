using UnityEngine;

abstract public class IEffects: MonoBehaviour
{
    
    public GameObject soundObject;

    float oldscale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        // Set init scale
        oldscale = 0;
        //Get SoundObject
        // Effect will be child of sound Object
        soundObject = transform.parent.gameObject;
        Init();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (transform.localScale.x != oldscale)
        setWet(transform.localScale.x);
        // Debug.Log("local scale = " + transform.localScale.x);
        // Debug.Log("old scale = " + oldscale);
        oldscale = transform.localScale.x;
        transform.RotateAround(soundObject.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
    public abstract void Init();
    public abstract void setWet(float wetness);
}
