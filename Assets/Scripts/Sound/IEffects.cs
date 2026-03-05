using UnityEngine;

abstract public class IEffects: MonoBehaviour
{
    
    public GameObject soundObject;

    float oldscale;
    public float rotationSpeed = 25.0f;

    private float maxDistanceToParent = 5.0f;
    [SerializeField] private float lerpSpeed = 7.5f;

    private bool active;

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
        // Debug.Log("local scale = " + transform.localScale.x);
        // Debug.Log("old scale = " + oldscale);
        oldscale = transform.localScale.x;

        if (active) return;
        // get direction to sound object
        Vector3 distance = (soundObject.transform.position - transform.position).normalized;
        //get perpendicular vector between direction and forward in order for the rotateAround to rotate in this direction.
        Vector3 direction = Vector3.Cross(distance, transform.forward);
        
        transform.RotateAround(soundObject.transform.position, direction, rotationSpeed * Time.deltaTime);

        // get direction to sound object
        direction = (soundObject.transform.position - transform.position).normalized;
        //get perpendicular vector between direction and forward in order for the rotateAround to rotate in this direction.
        direction = Vector3.Cross(direction, transform.right);

        transform.RotateAround(soundObject.transform.position, direction, (rotationSpeed/-3) * Time.deltaTime);

        if (distance.magnitude > maxDistanceToParent) LerpToMaxDistance();
    }
    public abstract void Init();
    public abstract void setWet(float wetness);


    public void LerpToMaxDistance()
    {
        Vector3 offset = transform.position - soundObject.transform.position;
        float distance = offset.magnitude;

        if (distance > maxDistanceToParent)
        {
            Vector3 targetPos = soundObject.transform.position + offset.normalized * maxDistanceToParent;

            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * lerpSpeed
            );
        }
    }

    public void setActive(bool active)
    {
        this.active = active;
    }
}
