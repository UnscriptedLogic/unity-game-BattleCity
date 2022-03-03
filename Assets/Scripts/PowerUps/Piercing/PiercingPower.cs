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
        manager.damage += amount;
        manager.bulletType = BulletType.Piercing;
        manager.GetComponent<EntityHealth>().onKilled += DestroySelf;
    }

    private void DestroySelf()
    {
        manager.damage -= increaseAmount;
        manager.damage = manager.damage <= 0 ? 1 : manager.damage;

        manager.GetComponent<EntityHealth>().onKilled -= DestroySelf;
        Destroy(this);
    }
}