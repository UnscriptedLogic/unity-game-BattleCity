using UnityEngine;

public enum CreateOn
{
    Spawn,
    Hurt,
    Destroyed,
}

[System.Serializable]
public class MiscEffects
{
    public CreateOn createOn;
    public GameObject[] particles;
    public AudioClip[] sounds;
    public int priority = 225;
    public float volume = 0.5f;
    public int maxDistance = 10;
    public float spatialBlend = 1f;
}

[CreateAssetMenu(fileName = "EntitySettings", menuName = "ScriptableObjects/Entity Settings")]
public class EntitySettings : ScriptableObject
{
    public int health = 1;
    public float speed = 10f;
    public float rotationSpeed = 30f;
    public int damage = 1;

    [Header("Miscellaneous. Can be left null.")]
    public MiscEffects[] miscEffects;
}