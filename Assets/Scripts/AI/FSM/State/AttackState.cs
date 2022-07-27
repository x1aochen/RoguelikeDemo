using UnityEngine;

namespace AI
{
    public class AttackState : BaseState
    {
        private float atkTimer;
        public override void Init()
        {
            stateID = FSMStateID.Attack;
        }

        public override void EnterState(BaseFSM fsm)
        {
            atkTimer = fsm.data.atkInterval;
        }

        public override void ActionState(BaseFSM fsm)
        {
            //ʼ��ת��Ŀ���
            fsm.FaceToTarget(fsm.target.position);
            if (atkTimer > fsm.data.atkInterval)
            {
                fsm.Attack();
                fsm.anim.SetTrigger(Consts.enemyAnimPramAtk);
                atkTimer = 0;
            }
            atkTimer += Time.deltaTime;
        }
    }
}