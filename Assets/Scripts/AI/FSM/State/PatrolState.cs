using System;
using UnityEngine;

namespace AI
{
    public class PatrolState : BaseState
    {
        private int index;
        private int length;
        public override void Init()
        {
            stateID = FSMStateID.Patrol;
        }

        public override void EnterState(BaseFSM fsm)
        {
            base.EnterState(fsm);
            fsm.anim.SetBool(Consts.enemyAnimPramRun, true);
            length = fsm.wayPoint.Length;
            index = 0;
            fsm.MoveToTarget(fsm.wayPoint[index], fsm.data.moveSpeed);
        }

        public override void ActionState(BaseFSM fsm)
        {
            base.ActionState(fsm);
            //RandomPatrol(fsm);
            LoopPatrol(fsm);

        }
        /// <summary>
        /// 在路点上随机巡逻
        /// </summary>
        /// <param name="fsm"></param>
        private void RandomPatrol(BaseFSM fsm)
        {
            if (Vector3.Distance(fsm.transform.position,fsm.wayPoint[index]) < 1f)
            {
                index = CodeHelper.Random(0, length);
            }
            fsm.MoveToTarget(fsm.wayPoint[index], fsm.data.moveSpeed);
        }

        private void LoopPatrol(BaseFSM fsm)
        {
            if (Vector3.Distance(fsm.transform.position,fsm.wayPoint[index]) <= 1.3)
            {
                index = (index + 1) % length;
                fsm.MoveToTarget(fsm.wayPoint[index], fsm.data.moveSpeed); 
            }
        }

        public override void ExitState(BaseFSM fsm)
        {
            base.ExitState(fsm);
            fsm.StopMove();
            fsm.anim.SetBool(Consts.enemyAnimPramRun, false);
        }
    }
}