namespace AI
{
    public class DeadState : BaseState
    {
        public override void Init()
        {
            stateID = FSMStateID.Dead;
        }

        public override void EnterState(BaseFSM fsm)
        {
            fsm.StopMove();
            fsm.gameObject.SetActive(false);
        }
    }
}