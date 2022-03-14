using System.Collections;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    private GameObject fakeColliderGO;
    private BoxCollider boxCollider;

    public void Start()
    {
        //Create a gameobject to house the fake collider
        fakeColliderGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        fakeColliderGO.transform.SetParent(transform);
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