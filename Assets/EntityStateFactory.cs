public class EntityStateFactory
{
    EntityStateMachine _context;

    public EntityStateFactory(EntityStateMachine context)
    {
        _context = context;
    }

    public EntityBaseState Roam() { return new EntityRoamState(_context, this); }
    public EntityBaseState Chase() { return new EntityChaseState(_context, this); }
    public EntityBaseState Snipe() { return new EntitySnipeState(_context, this); }
}
