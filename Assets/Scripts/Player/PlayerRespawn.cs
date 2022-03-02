using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : EntitySemaphore
{
    public EntityHealth entityHealth;

    [Tooltip("Enable this to automatically set the spawn point as the scene initialization position")]
    public bool spawnAtStart;
    public Vector3 spawnPosition;

    public float spawnDelay = 2f;
    protected float _spawnDelay = 2f;
    
    public GameObject gfxGameobject;
    public BoxCollider boxCollider;
    public Rigidbody rb;
    public Behaviour[] toggleBehaviours;

    protected bool isDead;

    public event Action onPlayerRespawned;

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

    //private void Update()
    //{
    //    if (isDead)
    //    {
    //        if (_spawnDelay <= 0)
    //        {
    //            transform.position = spawnPosition;
    //            transform.rotation = Quaternion.Euler(Vector3.zero);

    //            isDead = false;
    //            TogglePlayer(true);
    //        } else {
    //            _spawnDelay -= Time.deltaTime;
    //        }
    //    }
    //}

    //private void DisablePlayer(EntityManager source)
    //{
    //    TogglePlayer(false);
    //    isDead = true;
    //    _spawnDelay = spawnDelay;
    //    //StartCoroutine(RespawnAfterDelay());
    //}

    //private void TogglePlayer(bool value)
    //{
    //    if (!gfxGameobject)
    //    {
    //        Debug.Log("GFX not referenced in " + name, gameObject);

    //    } else
    //    {
    //        gfxGameobject.SetActive(value);
    //    }

    //    boxCollider.enabled = value;
    //    rb.isKinematic = !value;

    //    for (int i = 0; i < toggleBehaviours.Length; i++)
    //    {
    //        toggleBehaviours[i].enabled = value;
    //    }

    //    if (value)
    //    {
    //        onPlayerRespawned?.Invoke();
    //    }
    //}

    //private IEnumerator RespawnAfterDelay()
    //{
    //    TogglePlayer(false);

    //    yield return new WaitForSeconds(spawnDelay);

    //    transform.position = spawnPosition;
    //    transform.rotation = Quaternion.Euler(Vector3.zero);

    //    TogglePlayer(true);
    //}
}
