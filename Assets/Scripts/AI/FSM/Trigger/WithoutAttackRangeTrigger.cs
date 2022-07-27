using UnityEngine;

namespace AI
{
    public class WithoutAttackRangeTrigger : BaseTrigger
    {
        public override bool HandleTrigger(BaseFSM fsm)
        {
            return Vector3.Distance(fsm.transform.position, fsm.target.position) > fsm.data.attackDist + 1;
        }

        public override void Init()
        {
            triggerID = FSMTriggerID.WithoutAttackRange;
        }
    }
}