using UnityEngine;

abstract public class IEffects: MonoBehaviour
{
    
    public GameObject soundObject;

    float oldscale;
    public float rotationSpeed = 25.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        // Set init scale
        oldscale = 0;
        //Get SoundObject
        // Effect will be child of sound Object
        soundObject = transform.parent.gameObject;
        Init();
        int negativo = (Random.value < 0.5f) ? -1 : 1;
        rotationSpeed *= negativo;

        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (transform.localScale.x != oldscale)
        setWet(transform.localScale.x);
         
        // Debug.Log("old scale = " + oldscale);
        oldscale = transform.localScale.x;
        // get direction to sound object
        Vector3 direction = (soundObject.transform.position - transform.position).normalized;
        //get perpendicular vector between direction and forward in order for the rotateAround to rotate in this direction.
        direction = Vector3.Cross(direction, transform.forward);
        
        transform.RotateAround(soundObject.transform.position, direction, rotationSpeed * Time.deltaTime);

        // get direction to sound object
        direction = (soundObject.transform.position - transform.position).normalized;
        //get perpendicular vector between direction and forward in order for the rotateAround to rotate in this direction.
        direction = Vector3.Cross(direction, transform.right);

        transform.RotateAround(soundObject.transform.position, direction, (rotationSpeed/-3) * Time.deltaTime);
    }
    public abstract void Init();
    public abstract void setWet(float wetness);
}
