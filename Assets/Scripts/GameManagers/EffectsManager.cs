using System.Collections;
using UnityEngine;

/*
    AudioDetails details = new AudioDetails(
    clip: ,
    volume: ,
    priority: ,
    maxDistance: ,
    loop: ,
    spatialBlend:
    );
 */

public class AudioDetails 
{
    public AudioClip clip;
    public float volume;
    public int priority;
    public float maxDistance;
    public float spatialBlend;
    public bool loop;
    public int spread;
    public AudioRolloffMode rolloffMode;

    public AudioDetails(AudioClip clip, float volume = 0.5f, int priority = 200, float maxDistance = 30, float spatialBlend = 1, bool loop = false, int spread = 360, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear)
    {
        this.clip = clip;
        this.volume = volume;
        this.priority = priority;
        this.maxDistance = maxDistance;
        this.spatialBlend = spatialBlend;
        this.loop = loop;
        this.spread = spread;
        this.rolloffMode = rolloffMode;
    }
}

public static class EffectsManager
{
    public static void CreateAudioGameObject(AudioDetails details, Vector3 position, bool destroy = true, Transform attachedTo = null)
    {
        GameObject soundObject;
        if (!attachedTo)
        {
            soundObject = new GameObject("AudioSource");
            soundObject.transform.position = position;
        } else
        {
            soundObject = attachedTo.gameObject;
        }

        AudioSource source = soundObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = details.clip;
        source.volume = details.volume;
        source.priority = details.priority;
        source.minDistance = 0f;
        source.maxDistance = details.maxDistance;
        source.spatialBlend = details.spatialBlend;
        source.loop = details.loop;
        source.spread = details.spread;
        source.rolloffMode = details.rolloffMode;

        source.Play();

        if (destroy)
        {
            Object.Destroy(soundObject, details.clip.length);
        }
    }

    public static void CreateAudioGameObject(AudioDetails details, Vector3 position, out AudioSource source, bool destroy = true, Transform attachedTo = null)
    {
        GameObject soundObject = new GameObject("AudioSource");
        soundObject.transform.position = position;
        if (attachedTo)
        {
            soundObject.transform.SetParent(attachedTo);
        }

        source = soundObject.AddComponent<AudioSource>();
        source.clip = details.clip;
        source.volume = details.volume;
        source.priority = details.priority;
        source.maxDistance = details.maxDistance;
        source.spatialBlend = details.spatialBlend;
        source.loop = details.loop;
        source.rolloffMode = details.rolloffMode;

        source.Play();

        if (destroy)
        {
            Object.Destroy(soundObject, details.clip.length);
        }
    }

    public static void CreateParticlesGameObject(GameObject prefab, Vector3 position, Quaternion rotation, bool destroy = true, Transform attachedTo = null)
    {
        GameObject particle = Object.Instantiate(prefab, position, rotation, attachedTo);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

        if (destroy)
        {
            Object.Destroy(particle, particleSystem.main.startLifetime.constantMax);
        }
    }
}