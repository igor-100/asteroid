namespace Core.State
{
    public abstract class State
    {
        protected IStateable stateableObj;
        protected StateMachine stateMachine;

        protected State(IStateable stateableObj)
        {
            this.stateableObj = stateableObj;
            this.stateMachine = stateableObj.StateMachine;
        }

        public virtual void Enter()
        {

        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void PhysicsUpdate()
        {

        }

        public virtual void Exit()
        {

        }
    }
}