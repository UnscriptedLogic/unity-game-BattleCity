using System.Collections;
using UnityEngine;
using System.Threading;

public class BulletManager : EntityManager
{
    public int teamIndex;
    
    public Renderer matRenderer;
    public TankManager origin;
    public Rigidbody rb;

    [HideInInspector] public float lifetime;
    [HideInInspector] public BulletType bulletEffectors;

    public override void InitializeEntity()
    {
        //Bullet stats are initialized when spawned by the shooter
    }

    protected override void Start()
    {
        base.Start();

        //Sets the bullet colour to the associated team colour
        matRenderer.material.color = TeamManager.instance.teams[teamIndex].associatedColor;
    }
}