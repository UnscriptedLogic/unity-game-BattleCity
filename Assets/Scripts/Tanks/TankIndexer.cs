using System.Collections;
using UnityEngine;

public class TankIndexer : EntitySemaphore
{
    public TankManager tankManager;

    public int entityIndex;

    public override void Initialize(EntityManager manager)
    {
        tankManager.tankIndex = IndexManager.instance.SetNewEntity(transform);

        base.Initialize(manager);
    }

}