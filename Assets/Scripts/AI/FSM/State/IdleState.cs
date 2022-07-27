using UnityEngine;

namespace AI
{
    public class IdleState : BaseState
    {
        public override void Init()
        {
            stateID = FSMStateID.Idle;
        }

        public override void EnterState(BaseFSM fsm)
        {
            fsm.anim.SetBool(Consts.enemyAnimPramRun, false);
        }
    }
}