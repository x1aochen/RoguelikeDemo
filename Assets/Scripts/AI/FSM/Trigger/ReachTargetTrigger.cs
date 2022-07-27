using UnityEngine;

namespace AI
{
    public class ReachTargetTrigger : BaseTrigger
    {
        public override bool HandleTrigger(BaseFSM fsm)
        {
            return Vector3.Distance(fsm.transform.position, fsm.target.position) <= fsm.data.attackDist;
        }

        public override void Init()
        {
            triggerID = FSMTriggerID.ReachTarget;
        }
    }
}