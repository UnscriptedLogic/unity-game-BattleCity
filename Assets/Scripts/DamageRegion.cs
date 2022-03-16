using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRegion : MonoBehaviour
{
    public TankManager tankManager;
    public float damageInterval = 0.5f;
    private float _interval;

    private void Update()
    {
        if (_interval >= -1f)
        {
            _interval -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_interval <= 0f)
        {
            if (other as BoxCollider)
            {
                EntityManager entityManager = other.GetComponent<EntityManager>();
                if (entityManager)
                {
                    _interval = damageInterval;
                    DamageManager.DealDamage(amount: tankManager.damage, victim: entityManager, tankIndex: tankManager.tankIndex);
                    return;
                }
            }

        }
    }
}
