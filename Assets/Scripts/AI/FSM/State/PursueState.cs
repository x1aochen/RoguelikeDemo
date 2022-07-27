using UnityEngine;

namespace AI
{
    public class PursueState : BaseState
    {
        public override void Init()
        {
            stateID = FSMStateID.Pursue;
        }

        public override void EnterState(BaseFSM fsm)
        {
            fsm.anim.SetBool(Consts.enemyAnimPramRun, true);
        }
        float timer = 0;
        public override void ActionState(BaseFSM fsm)
        {
            //不需要每帧判定
            if (timer > 1)
            {
                fsm.MoveToTarget(fsm.target.position, fsm.data.runSpeed);
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        public override void ExitState(BaseFSM fsm)
        {
            fsm.anim.SetBool(Consts.enemyAnimPramRun, false);
            fsm.StopMove();
        }
    }
}