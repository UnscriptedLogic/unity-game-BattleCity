using System.Collections;
using UnityEngine;
using System.Threading;

public class BulletManager : EntityManager
{
    [Header("Components")]
    public BulletHealth healthScript;
    public BulletMovement movementScript;
    public Renderer matRenderer;
    public Rigidbody rb;

    public TankManager origin;

    public int teamIndex;
    [HideInInspector] public float lifetime;
    [HideInInspector] public BulletType bulletEffectors;

    protected override void OnEnable()
    {
        
    }

    private void Start()
    {
        //Sets the bullet colour to the associated team colour
        matRenderer.material.color = TeamManager.instance.teams[teamIndex].associatedColor;
    }

    public void SettingsInitialized()
    {
        base.OnEnable();
    }

    protected override void Initialize()
    {

    }
}