public class EntityStateFactory
{
    EntityStateMachine _context;

    public EntityStateFactory(EntityStateMachine context)
    {
        _context = context;
    }

    public EntityBaseState Roam() { return new EntityRoamState(_context, this); }
    public EntityBaseState ChaseTarget() { return new EntityChaseTargetState(_context, this); }
    public EntityBaseState ChaseBase() { return new EntityChaseBaseState(_context, this); }
    public EntityBaseState Snipe() { return new EntitySnipeState(_context, this); }
    public EntityBaseState Hold() { return new EntityHoldState(_context, this); }
    public EntityBaseState Idle() { return new EntityIdleState(_context, this); }

    public EntityBaseState BoreDash() { return new GiantBoreDashState(_context, this); }
    public EntityBaseState BoreChaseBase() { return new GiantBoreChaseBaseState(_context, this); }
    public EntityBaseState BoreHold() { return new GiantBoreHoldState(_context, this); }
    public EntityBaseState BoreIdle() { return new BoreIdleState(_context, this); }
}
