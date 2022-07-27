using UnityEngine;

namespace AI
{
    public class KilledTargetTrigger : BaseTrigger
    {
        public override bool HandleTrigger(BaseFSM fsm)
        {
            return fsm.targetAlive.IsAlive;
        }

        public override void Init()
        {
            triggerID = FSMTriggerID.KilledTarget;
        }
    }
}