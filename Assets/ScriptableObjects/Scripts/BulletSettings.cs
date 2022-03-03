using UnityEngine;

[CreateAssetMenu(fileName = "BulletSettings", menuName = "ScriptableObjects/Bullet Settings")]
public class BulletSettings : EntitySettings
{
    [Header("Bullet Settings")]
    public float lifetime = 10f;
    public BulletType bulletType;
}