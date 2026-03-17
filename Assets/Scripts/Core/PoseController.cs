using UnityEngine;
using SoundLab.Core;
using SoundLab.Sound;

public class PoseController : MonoBehaviour
{
    public bool sustain;
    public Vector3 volumePoseInit;
    public Transform handTransform;
    public bool volumeDown;
    public bool volumeUp;
    public float volumeScaleCoeff = 1f;
    public float distanceCoeff = 60f;
    public float maxDistance = 0.3f;
    public float prevVolume;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sustain = false;
        volumeDown = false;
        volumeUp = false;
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.AudioToChange.Count == 0) return;
        if (GameController.Instance.AudioToChange[0].gameObject.CompareTag("Instrument")) InstrumentUpdate();
        else realUpdate();
    }

    void InstrumentUpdate()
    {
        Debug.Log("Instrument volume update");
        var yOffset = handTransform.localPosition.y - volumePoseInit.y;

        if (volumeUp)
        {
                Debug.Log("would change volume by " + Remap(Mathf.Clamp(yOffset, 0, 0.3f), 0, 0.3f, prevVolume, 1f) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            //GameController.Instance.AudioToChange[0].volume += yOffset * 100000;
            GameController.Instance.Instrument.changeTargetVolume(Remap(Mathf.Clamp(yOffset, 0, 0.3f), 0, 0.3f, prevVolume, 1f));
            
        }
        if (volumeDown)
        {

            Debug.Log("would change volume to" + Remap(Mathf.Clamp(yOffset, -0.3f, 0f), -0.3f, 0f, 0, prevVolume) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            //GameController.Instance.AudioToChange[0].volume += yOffset * 100000;
            GameController.Instance.Instrument.changeTargetVolume(Remap(Mathf.Clamp(yOffset, -0.3f, 0f), -0.3f, 0f, 0f, prevVolume));

        }

    }
    void fakeInstrument()
    {
        Debug.Log("Instrument volume update");
        var yOffset = handTransform.localPosition.y - volumePoseInit.y;

        if (volumeUp)
        {
            if (yOffset == 0f) return;
            if (yOffset <= 0f)
            {
                volumePoseInit = handTransform.localPosition;
                //Debug.Log("Changing volumePoseInit to " + volumePoseInit.y);
                return;
            }
            Debug.Log("would change volume by " + Mathf.Clamp(yOffset * distanceCoeff, 0, volumeScaleCoeff * Time.deltaTime) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            //GameController.Instance.AudioToChange[0].volume += yOffset * 100000;
            GameController.Instance.Instrument.changeTargetVolume(Mathf.Clamp(yOffset * distanceCoeff, 0, volumeScaleCoeff * Time.deltaTime));

        }
        if (volumeDown)
        {
            if (yOffset == 0f) return;
            if (yOffset >= 0f)
            {
                volumePoseInit = handTransform.localPosition;
                //Debug.Log("Changing volumePoseInit to " + volumePoseInit.y);
                return;
            }
            Debug.Log("would change volume by " + Mathf.Clamp(yOffset * distanceCoeff, -volumeScaleCoeff * Time.deltaTime, 0f) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            //GameController.Instance.AudioToChange[0].volume += yOffset * 10000;
            GameController.Instance.Instrument.changeTargetVolume(Mathf.Clamp(yOffset * distanceCoeff, -volumeScaleCoeff * Time.deltaTime, 0));
        }
    }
    void realUpdate()
    {
        var yOffset = handTransform.localPosition.y - volumePoseInit.y;

        if (volumeUp)
        {

            Debug.Log("would change volume by " + Mathf.Clamp(yOffset * distanceCoeff, 0, volumeScaleCoeff * Time.deltaTime) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            //GameController.Instance.AudioToChange[0].volume += yOffset * 100000;
            GameController.Instance.AudioToChange[0].gameObject.GetComponent<SoundTrigger>().changeTargetVolume(Remap(Mathf.Clamp(yOffset, 0, 0.3f), 0, 0.3f, prevVolume, 1f));

        }
        if (volumeDown)
        {

            Debug.Log("would change volume by " + Mathf.Clamp(yOffset * distanceCoeff, 0, volumeScaleCoeff * Time.deltaTime) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            //GameController.Instance.AudioToChange[0].volume += yOffset * 100000;
            GameController.Instance.AudioToChange[0].gameObject.GetComponent<SoundTrigger>().changeTargetVolume(Remap(Mathf.Clamp(yOffset, -0.3f, 0f), -0.3f, 0f, 0f, prevVolume));

        }
    }

    void fakeUpdate()
    {
        if (GameController.Instance.AudioToChange.Count == 0) return;

        var yOffset = handTransform.localPosition.y - volumePoseInit.y;
        if (volumeUp)
        {
            if (yOffset == 0f) return;
            if (yOffset <= 0f)
            {
                volumePoseInit = handTransform.localPosition;
                //Debug.Log("Changing volumePoseInit to " + volumePoseInit.y);
                return;
            }
            Debug.Log("would change volume by " + Mathf.Clamp(yOffset * distanceCoeff, 0, volumeScaleCoeff * Time.deltaTime) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            // move total 0.3 shoul be a whole 1
            //GameController.Instance.AudioToChange[0].volume += yOffset * 100000;
            GameController.Instance.AudioToChange[0].gameObject.GetComponent<SoundTrigger>().changeTargetVolume(Mathf.Clamp(yOffset * distanceCoeff, 0, volumeScaleCoeff * Time.deltaTime));

        }
        if (volumeDown)
        {
            if (yOffset == 0f) return;
            if (yOffset >= 0f)
            {
                volumePoseInit = handTransform.localPosition;
                //Debug.Log("Changing volumePoseInit to " + volumePoseInit.y);
                return;
            }
            Debug.Log("would change volume by " + Mathf.Clamp(yOffset * distanceCoeff, -volumeScaleCoeff * Time.deltaTime, 0f) + "On Object " + GameController.Instance.AudioToChange[0].gameObject.name);
            //GameController.Instance.AudioToChange[0].volume += yOffset * 10000;
            GameController.Instance.AudioToChange[0].gameObject.GetComponent<SoundTrigger>().changeTargetVolume(Mathf.Clamp(yOffset * distanceCoeff ,-volumeScaleCoeff * Time.deltaTime,0));
        }
    }

    public void SustainInstrument()
    {
        sustain = !sustain;
        //GameController.Instance.Instrument.removeAudioSource(sustain);
        foreach (SoundTrigger sound in GameController.Instance.Sounds)
        {
            sound.Sustain(sustain);
        }
    }
    public void ResumeInstrument()
    {
        GameController.Instance.Instrument.addAudioSource();
    }

    public void VolumeUp(bool init)
    {
        if (!init)
        {
            volumeUp = init;
            Debug.Log("Volume Up finished");
            return;
        }
        volumePoseInit = handTransform.localPosition;
        volumeUp = init;
        if (GameController.Instance.AudioToChange[0].gameObject.CompareTag("Instrument"))
            prevVolume = GameController.Instance.AudioToChange[0].volume;
        else prevVolume = GameController.Instance.AudioToChange[0].gameObject.GetComponent<SoundTrigger>().getTargeVolume();
        Debug.Log("volumeUp init" + volumePoseInit);
    }
    public void VolumeDown(bool init)
    {
        
        if (!init)
        {
            volumeDown = init;
            Debug.Log("Volume Down finished");
            return;
        }
        volumePoseInit = handTransform.localPosition;
        Debug.Log("volumeDown init" + volumePoseInit);
        volumeDown = init;
        if (GameController.Instance.AudioToChange[0].gameObject.CompareTag("Instrument"))
            prevVolume = GameController.Instance.AudioToChange[0].volume;
        else prevVolume = GameController.Instance.AudioToChange[0].gameObject.GetComponent<SoundTrigger>().getTargeVolume();
        Debug.Log(" volume down is" + volumeDown + " init is " + init);

    }

}
