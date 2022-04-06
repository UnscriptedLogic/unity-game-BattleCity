using System.Collections;
using UnityEngine;

public class PiercingPower : MonoBehaviour
{
    private TankManager manager;
    private int increaseAmount;
    private GameObject particle;

    private void Start()
    {
        particle = Instantiate(AssetManager.instance.piercingAura, transform);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
            particle.transform.position = hit.point;
        }

        BoxCollider boxCollider = transform.GetComponent<BoxCollider>();
        if (boxCollider)
        {
            float scale = Mathf.Max(boxCollider.size.x, boxCollider.size.z) / 1.5f;
            particle.transform.localScale = new Vector3(scale, 1f, scale);
        }
    }

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
        Destroy(particle);
        Destroy(this);
    }
}