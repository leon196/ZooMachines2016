using UnityEngine;
using System.Collections;
using MidiJack;
using UnityEngine.Audio;

public class PitchBend : MonoBehaviour
{

    public AudioClip c;

    public SfxrSynth synth;
    public SfxrParams param;

    public AudioMixerGroup group;

    void Update()
    {
        for (int i = 0; i < 128; i++)
        {
            if (!MidiMaster.GetKeyDown(i))
            {
                continue;
            }

            //Debug.Log("Pressed " + i);

            float value = MidiMaster.GetKey(i);
            value = 1;
            float pitch = Mathf.Pow(2, (i - 48) / 12f);

            AudioSource src = c.PlayOnce(Vector3.zero, value, pitch, 90, 10001);
            src.outputAudioMixerGroup = group;
        }
    }
}
