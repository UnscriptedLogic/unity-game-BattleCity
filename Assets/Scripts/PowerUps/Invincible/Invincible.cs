using System.Collections;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    public float duration;
    public GameObject fakeColliderGO;
    public BoxCollider realCollder;

    public void Start()
    {
        realCollder.enabled = false;
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(duration);
        realCollder.enabled = true;
        Destroy(fakeColliderGO);
        Destroy(this);
    }
}