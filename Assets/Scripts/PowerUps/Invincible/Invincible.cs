using System.Collections;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    public float duration;
    public TankManager shieldOwner;

    public void Start()
    {
        StartCoroutine(LifeTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        BulletManager manager = other.GetComponent<BulletManager>();
        if (manager)
        {
            if (manager.origin != shieldOwner)
            {
                Destroy(other.gameObject);
            }
        }
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
        
    private void OnDestroy()
    {

    }
}