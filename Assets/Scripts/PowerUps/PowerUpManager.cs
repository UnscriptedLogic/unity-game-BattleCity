using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    ArmorPiercing,
    Invincible,
    Time,
    Nuke,
    HomeFortify
}

public class PowerUpManager : EntityManager
{
    public EntityHealth entityHealth;
    public PowerUpType powerUpType;

    [Tooltip("Check this if bot tanks can pick this up.")]
    public bool botTanks;

    [Tooltip("Check this if player tanks can pick this up.")]
    public bool playerTanks;
    
    [Tooltip("Set to -1 to prevent it from despawning")]
    public float lifeTime = 8;
    protected TankManager collectorManager;
    protected GameManager gameManager;

    public event Action<TankManager> onPowerUp;

    protected float _lifetime;
    protected bool notDespawning;

    public override void InitializeEntity()
    {
        base.InitializeEntity();
        gameManager = GameManager.instance;
        _lifetime = lifeTime;
        notDespawning = lifeTime == -1f ? true : false;
    }

    protected virtual void Update()
    {
        if (notDespawning)
        {
            return;
        }

        if (_lifetime <= 0)
        {
            Destroy(gameObject);
        } else
        {
            _lifetime -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collectorManager = collision.transform.GetComponent<TankManager>();
        if (collectorManager)
        {
            if (botTanks && collision.transform.GetComponent<BotManager>())
            {
                collectorManager = collision.transform.GetComponent<BotManager>();
                Activate(collision);
            }

            if (playerTanks && collision.transform.GetComponent<PlayerManager>())
            {
                collectorManager = collision.transform.GetComponent<PlayerManager>();
                Activate(collision);
            }

            onPowerUp?.Invoke(collectorManager);
            return;
        }
    }

    protected void AffectNotOnTeam(Collision collision, Action method)
    {
        TankTeamIndexer collectorIndexer = collision.transform.GetComponent<TankTeamIndexer>();
        List<TankManager> collectorEnemies = new List<TankManager>();
        collectorEnemies = TeamManager.instance.GetTanksNotInTeam(collectorIndexer.teamIndex);

        for (int i = 0; i < collectorEnemies.Count; i++)
        {
            method();
        }
    }

    protected virtual void Activate(Collision collision)
    {
        if (gameManager.playSounds)
        {
            MiscEffects miscEffects = RandomValue.FromList(settings.miscEffects);
            AudioDetails details = new AudioDetails(
                clip: RandomValue.FromList(miscEffects.sounds),
                volume: miscEffects.volume,
                priority: miscEffects.priority,
                maxDistance: miscEffects.maxDistance,
                spatialBlend: miscEffects.spatialBlend
                );
            EffectsManager.CreateAudioGameObject(details, transform.position);
        }
    }

    protected void SelfDestruct()
    {
        //transform.GetComponent<EntityHealth>().TakeDamage(999);
        entityHealth.KillEntity();
    }
}
