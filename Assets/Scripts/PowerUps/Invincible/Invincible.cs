using System.Collections;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    private GameObject fakeColliderGO;
    private BoxCollider boxCollider;
    public float duration;

    public void Start()
    {
        Invincible[] invincibleScripts = transform.GetComponents<Invincible>();
        for (int i = 0; i < invincibleScripts.Length; i++)
        {
            if (invincibleScripts[i] != this)
            {
                invincibleScripts[i].Activate(invincibleScripts[i].duration);
                Destroy(this);
                return;
            }
        }

        //Create a gameobject to house the fake collider
        fakeColliderGO = Instantiate(AssetManager.instance.forcefield, transform);
        fakeColliderGO.transform.position = transform.position;
        fakeColliderGO.transform.localScale = Vector3.one;

        //Create a fake collider - damage scripts will look for an entityhealth. Seeing there's none, no damage done
        boxCollider = transform.GetComponent<BoxCollider>();
        BoxCollider fakeBoxCollider = fakeColliderGO.AddComponent<BoxCollider>();
        fakeBoxCollider.size = boxCollider.size;
        fakeBoxCollider.center = boxCollider.center;
        fakeBoxCollider.isTrigger = false;

        boxCollider.enabled = false;
    }

    public void Activate(float duration)
    {
        this.duration = duration;
        StopAllCoroutines();
        StartCoroutine(LifeTime(duration));
    }

    private IEnumerator LifeTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        boxCollider.enabled = true;
        Destroy(fakeColliderGO);
        Destroy(this);
    }
}