using System.Collections;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    private GameObject fakeColliderGO;
    private BoxCollider boxCollider;
    public float duration;

    public bool overrideDuration;
    public GameObject overrideGFX;

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

        if (overrideDuration)
        {
            Activate(duration);
        }
    }

    public void Activate(float duration)
    {
        this.duration = duration;
        StopAllCoroutines();
        StartCoroutine(LifeTime(duration));

        //Create a gameobject to house the fake collider
        if (overrideGFX != null)
        {
            fakeColliderGO = Instantiate(overrideGFX, transform);
        }
        else
        {
            fakeColliderGO = Instantiate(AssetManager.instance.forcefield, transform);
        }

        fakeColliderGO.transform.position = transform.position;
        fakeColliderGO.transform.localScale = Vector3.one;

        //Create a fake collider - damage scripts will look for an entityhealth. Seeing there's none, no damage done
        boxCollider = transform.GetComponent<BoxCollider>();
        BoxCollider fakeBoxCollider = fakeColliderGO.AddComponent<BoxCollider>();
        fakeColliderGO.layer = LayerMask.NameToLayer("Ignore Raycast"); //Ignores raycast for any damage
        fakeBoxCollider.size = boxCollider.size;
        fakeBoxCollider.center = boxCollider.center;
        fakeBoxCollider.isTrigger = false;

        boxCollider.enabled = false;
    }

    private IEnumerator LifeTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        boxCollider.enabled = true;
        Destroy(fakeColliderGO);
    }
}