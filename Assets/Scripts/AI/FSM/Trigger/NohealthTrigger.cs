using UnityEngine;

namespace AI
{
    public class NohealthTrigger : BaseTrigger
    {
        public override bool HandleTrigger(BaseFSM fsm)
        {
            return fsm.data.hp <= 0;
        }

        public override void Init()
        {
            triggerID = FSMTriggerID.Nohealth;
        }
    }
}