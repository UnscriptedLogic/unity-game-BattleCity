using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRegion : MonoBehaviour
{
    public TankManager tankManager;
    public float damageInterval = 0.5f;
    
    private float _interval;
    public float checkRadius;
    public float checkDistance;

    private void Update()
    {
        if (_interval <= 0f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, checkRadius, out RaycastHit hitInfo, checkDistance))
            {
                BoxCollider boxCollider = hitInfo.transform.GetComponent<BoxCollider>();
                if (boxCollider)
                {
                    EntityManager entityManager = hitInfo.transform.GetComponent<EntityManager>();
                    if (entityManager)
                    {
                        _interval = damageInterval;
                        DamageManager.DealDamage(amount: tankManager.damage, victim: entityManager, tankIndex: tankManager.tankIndex);
                    }
                }
            }
        } else
        {
            _interval -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, transform.forward * checkDistance);
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
