//#define ENABLE_SPATIALIZER_API // remove if no spatializer
//#define AUDIO_MANAGER

using UnityEngine;
using System.Collections;

public static class AudioUtils
{
    const float standardMinDistance = 1;
    const float standardSpread = 90;

    public static AudioSource PlayRandomOnce(AudioClip[] clips, Vector3 position, float voluem = 1, float pitch = 1, float spread = standardSpread, float minDistance = standardMinDistance)
    {
        return PlayOnce(clips[Random.Range(0, clips.Length)], position, voluem, pitch, spread, minDistance);
    }

    public static AudioSource PlayOnce(this AudioClip clip, Vector3 position, float volume = 1, float pitch = 1, float spread = standardSpread, float minDistance = standardMinDistance)
    {
        GameObject go = new GameObject("AudioTemp");
        go.transform.position = position;

        AudioSource source = go.AddComponent<AudioSource>();

        source.spatialBlend = 1; // makes the source 3d
        source.minDistance = minDistance;

        source.loop = false;
        source.clip = clip;

        source.volume = volume;
        source.pitch = pitch;
        source.spread = spread;

        source.dopplerLevel = 0;

#if ENABLE_SPATIALIZER_API
        source.spatialize = true;
#endif

#if AUDIO_MANAGER
        source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, AudioManager.e.reverbCurve);
        source.outputAudioMixerGroup = AudioManager.e.currentGroup;
#endif

        source.Play();

        if (pitch == 0) pitch = 100;
        GameObject.Destroy(source.gameObject, clip.length * (1 / pitch));

        return source;
    }

    // AUDIO SOURCE CREATION

    public static AudioSource CreateSource(Transform at, AudioClip clip = null, bool loop = true, bool playAtStart = false,
                                            float minDistance = standardMinDistance, float volume = 1, float pitch = 1, float spread = standardMinDistance, float spatialBlend = 1)
    {
        GameObject go = new GameObject("AudioLoop");
        go.transform.parent = at;
        go.transform.localPosition = Vector3.zero;

        AudioSource source = go.AddComponent<AudioSource>();

        source.loop = loop;
        source.clip = clip;

        source.volume = volume;
        source.spatialBlend = spatialBlend;
        source.spread = spread;
        source.minDistance = minDistance;

        source.playOnAwake = playAtStart;

        return source;
    }
}
