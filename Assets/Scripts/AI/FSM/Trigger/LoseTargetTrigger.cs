using UnityEngine;

namespace AI
{
    public class LoseTargetTrigger : BaseTrigger
    {
        public override bool HandleTrigger(BaseFSM fsm)
        {
            return !fsm.findTarget;
        }

        public override void Init()
        {
            triggerID = FSMTriggerID.LoseTarget;
        }
    }
}
