using UnityEngine;

[CreateAssetMenu(fileName = "New EntitySettings", menuName = "ScriptableObjects/Tank Settings")]
public class TankSettings : EntitySettings
{
    [Header("Attack Settings")]
    public BulletSettings bulletSettings;
    public GameObject bulletPrefab;
}