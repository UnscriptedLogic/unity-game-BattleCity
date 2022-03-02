using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : Semaphore
{
    private TankManager tankManager;
    public EntityHealth entityHealth;

    [Tooltip("Enable this to automatically set the spawn point as the scene initialization position")]
    public bool spawnAtStart;
    public Vector3 spawnPosition;

    public float spawnDelay = 2f;
    protected float _spawnDelay = 2f;
    
    public Rigidbody rb;
    public BoxCollider boxCollider;
    public GameObject gfxGameobject;
    
    public Behaviour[] toggleBehaviours;

    protected bool isDead;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        tankManager = manager as TankManager;

        if (spawnAtStart)
        {
            spawnPosition = transform.position;
        }

        entityHealth.onKilled += EntityHealth_onKilled;
    }

    private void EntityHealth_onKilled()
    {
        isDead = true;
        _spawnDelay = spawnDelay;
        TogglePlayer(false);
    }

    //public override void Initialize(EntityManager manager)
    //{
    //    if (spawnAtStart)
    //    {
    //        spawnPosition = transform.position;
    //    }

    //    if (!entityHealth)
    //    {
    //        entityHealth = GetComponent<EntityHealth>();
    //        if (!entityHealth)
    //        {
    //            Debug.LogWarning("The GameObject " + name + " could not find EntityHealth");
    //            return;
    //        }
    //    }

    //    entityHealth.onKilled += DisablePlayer;

    //    base.Initialize(manager);
    //}

    private void Update()
    {
        if (isDead)
        {
            if (_spawnDelay <= 0)
            {
                tankManager.InitializeEntity();

                transform.position = spawnPosition;
                transform.rotation = Quaternion.Euler(Vector3.zero);

                isDead = false;
                TogglePlayer(true);
            }
            else
            {
                _spawnDelay -= Time.deltaTime;
            }
        }
    }

    //private void DisablePlayer(EntityManager source)
    //{
    //    TogglePlayer(false);
    //    isDead = true;
    //    _spawnDelay = spawnDelay;
    //    //StartCoroutine(RespawnAfterDelay());
    //}

    private void TogglePlayer(bool value)
    {
        if (!gfxGameobject)
        {
            Debug.Log("GFX not referenced in " + name, gameObject);

        }
        else
        {
            gfxGameobject.SetActive(value);
        }

        boxCollider.enabled = value;
        rb.isKinematic = !value;

        for (int i = 0; i < toggleBehaviours.Length; i++)
        {
            toggleBehaviours[i].enabled = value;
        }
    }

    //private IEnumerator RespawnAfterDelay()
    //{
    //    yield return new WaitForSeconds(spawnDelay);

    //    transform.position = spawnPosition;
    //    transform.rotation = Quaternion.Euler(Vector3.zero);

    //    TogglePlayer(true);
    //}
}
