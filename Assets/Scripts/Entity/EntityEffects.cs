using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DependantState
{
    WhileAlive,
    WhileStationary,
    WhileMoving,
    OnCollision
}

[System.Serializable]
public class DependantFX
{
    public DependantState dependantState;
    public GameObject[] particles;
    public AudioClip[] sounds;
    public int priority = 225;
    public float volume = 0.5f;
    public int maxDistance = 10;
    public float spatialBlend = 1f;
}

public class EntityEffects : Semaphore
{
    private GameManager gameManager;
    private EntityManager entityManager;

    public EntityMovement movementScript; 
    public EntityHealth entityHealth;

    private MiscEffects[] miscFXes;

    public float checkDistance;
    public bool randomRotation;
    public bool activated;

    private List<AudioSource> whileMovingSounds = new List<AudioSource>();
    private List<ParticleSystem> whileMovingParticles = new List<ParticleSystem>();

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        gameManager = GameManager.instance;

        entityManager = manager as EntityManager;
        miscFXes = entityManager.settings.miscEffects;
    }

    public void InitializeEvents()
    {
        entityHealth.onKilled += delegate ()
        {
            PlayOneShot(CreateOn.Destroyed);
        };

        entityHealth.onHealthDeducted += delegate (int amount)
        {
            PlayOneShot(CreateOn.Hurt);
        };

        //For sound effects that only play on spawn
        PlayOneShot(CreateOn.Spawn);
    }

    public void PlayOneShot(CreateOn createOn)
    {
        for (int i = 0; i < miscFXes.Length; i++)
        {
            if (miscFXes[i].createOn == createOn)
            {
                CreateParticlesGO(miscFXes[i]);
                CreateSoundGO(miscFXes[i], true);
            }
        }
    }

    //public override void Initialize(EntityManager manager)
    //{
    //    healthScript.onKilled += PlayDestroyedFX;
    //    healthScript.onHealthDeducted += PlayOnHurt;

    //    PlayOnCreate();

    //    if (movementScript != null)
    //    {
    //        movementScript.onEntityMove += PlayEntityMovedFX;
    //        for (int i = 0; i < dependantFXes.Length; i++)
    //        {
    //            if (dependantFXes[i].dependantState == DependantState.WhileMoving)
    //            {
    //                AudioDetails details = new AudioDetails(
    //                    clip: RandomValue.FromList(dependantFXes[i].sounds),
    //                    volume: dependantFXes[i].volume,
    //                    priority: dependantFXes[i].priority,
    //                    maxDistance: dependantFXes[i].maxDistance,
    //                    loop: true,
    //                    spatialBlend: dependantFXes[i].spatialBlend
    //                );

    //                EffectsManager.CreateAudioGameObject(details, transform.position, source: out AudioSource source, false, attachedTo: transform);
    //                whileMovingSounds.Add(source);
    //                source.Stop();

    //                for (int j = 0; j < dependantFXes[i].particles.Length; j++)
    //                {
    //                    ParticleSystem particleSystem = dependantFXes[i].particles[j].GetComponent<ParticleSystem>();
    //                    whileMovingParticles.Add(particleSystem);
    //                    particleSystem.Stop();
    //                }
    //            }
    //        }
    //    }

    //    initialized = true;
    //    base.Initialize(manager);
    //}

    //private void Update()
    //{
    //    if (gameManager.playParticles)
    //    {
    //        if (Physics.Raycast(origin: transform.position, direction: -transform.up, maxDistance: checkDistance) && !activated)
    //        {
    //            for (int i = 0; i < dependantFXes.Length; i++)
    //            {
    //                if (dependantFXes[i].dependantState == DependantState.OnCollision)
    //                {
    //                    EffectsManager.CreateParticlesGameObject(prefab: RandomValue.FromList(dependantFXes[i].particles), position: transform.position, rotation: Quaternion.identity);
    //                    activated = true;
    //                }
    //            }
    //        }
    //    }
    //}

    //private void PlayEntityMovedFX(bool value)
    //{
    //    if (value)
    //    {
    //        if (gameManager.playSounds)
    //        {
    //            for (int i = 0; i < whileMovingSounds.Count; i++)
    //            {
    //                whileMovingSounds[i].Play();
    //            }
    //        }

    //        if (gameManager.playParticles)
    //        {
    //            for (int j = 0; j < whileMovingParticles.Count; j++)
    //            {
    //                whileMovingParticles[j].Play();
    //            } 
    //        }
    //    } else
    //    {
    //        if (gameManager.playSounds)
    //        {
    //            for (int i = 0; i < whileMovingSounds.Count; i++)
    //            {
    //                whileMovingSounds[i].Stop();
    //            } 
    //        }

    //        if (gameManager.playParticles)
    //        {
    //            for (int j = 0; j < whileMovingParticles.Count; j++)
    //            {
    //                whileMovingParticles[j].Stop();
    //            } 
    //        }
    //    }
    //}

    //protected void PlayFX(CreateOn createOn)
    //{
    //    if (gameManager.playEffects)
    //    {
    //        for (int i = 0; i < miscFXes.Length; i++)
    //        {
    //            if (miscFXes[i].createOn == createOn)
    //            {
    //                CreateParticlesGO(miscFXes[i]);

    //                CreateSoundGO(miscFXes[i]);

    //            }
    //        }
    //    }
    //}

    //protected void PlayDestroyedFX(EntityManager source)
    //{
    //    PlayFX(CreateOn.Destroyed);
    //}

    //protected void PlayOnCreate()
    //{
    //    PlayFX(CreateOn.Spawn);
    //}

    //protected void PlayOnHurt(int amount)
    //{
    //    PlayFX(CreateOn.Hurt);
    //}

    public void CreateSoundGO(MiscEffects miscFX, bool destroy = true, Transform parent = null)
    {
        if (gameManager.playSounds && miscFX.sounds.Length > 0)
        {
            AudioDetails details = new AudioDetails(
                clip: RandomValue.FromList(miscFX.sounds),
                volume: miscFX.volume,
                priority: miscFX.priority,
                maxDistance: miscFX.maxDistance,
                spatialBlend: miscFX.spatialBlend
            );
            EffectsManager.CreateAudioGameObject(details: details, position: transform.position, destroy, parent);
        }
    }

    public void CreateParticlesGO(MiscEffects miscFX)
    {
        if (gameManager.playParticles && miscFX.particles.Length > 0)
        {
            GameObject particle = Instantiate(RandomValue.FromList(miscFX.particles), transform.position, transform.rotation);
            Destroy(particle, 10f);
        }
    }

    //public void CreateParticlesGO(DependantFX miscFX)
    //{
    //    if (gameManager.playParticles && miscFX.particles.Length > 0)
    //    {
    //        GameObject particle = Instantiate(RandomValue.FromList(miscFX.particles), transform.position, transform.rotation);
    //        Destroy(particle, 10f);
    //    }
    //}

    //public void CreateParticlesGO(DependantFX miscFX, Vector3 pos)
    //{
    //    if (gameManager.playParticles && miscFX.particles.Length > 0)
    //    {
    //        GameObject particle = Instantiate(RandomValue.FromList(miscFX.particles), pos, transform.rotation);
    //        Destroy(particle, 10f);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (gameManager.playSounds)
    //    {
    //        for (int i = 0; i < dependantFXes.Length; i++)
    //        {
    //            if (dependantFXes[i].dependantState == DependantState.OnCollision)
    //            {
    //                if (dependantFXes[i].sounds.Length > 0)
    //                {
    //                    AudioDetails details = new AudioDetails(
    //                        clip: RandomValue.FromList(dependantFXes[i].sounds),
    //                        volume: dependantFXes[i].volume,
    //                        priority: dependantFXes[i].priority,
    //                        maxDistance: dependantFXes[i].maxDistance,
    //                        spatialBlend: dependantFXes[i].spatialBlend
    //                    );

    //                    EffectsManager.CreateAudioGameObject(details, transform.position);
    //                }
    //            }
    //        }
    //    }
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawLine(transform.position, transform.position + (-transform.up * checkDistance));
    //}
}