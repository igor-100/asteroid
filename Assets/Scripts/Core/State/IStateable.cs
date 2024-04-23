namespace Core.State
{
    public interface IStateable
    {
        StateMachine StateMachine { get; }
    }
}
