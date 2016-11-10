using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using MidiJack;

public class KnobControl : MonoBehaviour
{

    public AudioMixer mixer;

    [System.Serializable]
    public class KnobProp
    {
        new public string name;
        public float min = 0;
        public float max = 1;
    }

    public KnobProp[] knobProps;

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < knobProps.Length; i++)
        {
            ProcessSound(i, knobProps[i].name, knobProps[i].min, knobProps[i].max);
        }
    }

    void ProcessSound(int slider, string propertyName, float min, float max)
    {

        if (string.IsNullOrEmpty(propertyName)) return;

        float value = Mathf.Lerp(min, max, MidiMaster.GetKnob(slider));
        mixer.SetFloat(propertyName, value);
    }
}