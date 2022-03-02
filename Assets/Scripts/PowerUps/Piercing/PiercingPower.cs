using System.Collections;
using UnityEngine;

public class PiercingPower : MonoBehaviour
{
    private TankManager manager;
    private int increaseAmount;

    public void ModifyAttack(int amount)
    {
        manager = GetComponent<TankManager>();
        increaseAmount += amount;
        manager.bulletHealth += amount;
        manager.bulletType = BulletType.Piercing;
        //manager.healthScript.onKilled += DestroySelf;
    }

    private void DestroySelf(EntityManager source)
    {
        manager.bulletHealth -= increaseAmount;
        manager.bulletHealth = manager.bulletHealth <= 0 ? 1 : manager.bulletHealth;

        //manager.healthScript.onKilled -= DestroySelf;
        Destroy(this);
    }
}