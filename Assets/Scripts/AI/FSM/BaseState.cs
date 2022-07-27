using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class BaseState
    {
        public FSMStateID stateID { get; set; }

        private Dictionary<FSMTriggerID, FSMStateID> map;

        private List<BaseTrigger> triggers;

        /// <summary>
        /// ��⵱ǰ״̬��Ӧ����
        /// </summary>
        /// <param name="fsm"></param>
        public void Reason(BaseFSM fsm)
        {
            foreach (var trigger in triggers)
            {
                if (trigger.HandleTrigger(fsm))
                {
                    //�����������л�
                    FSMStateID stateID = map[trigger.triggerID];

                    fsm.ChangeState(stateID);
                    return;
                }
            }
        }

        public BaseState()
        {
            Init();
            map = new Dictionary<FSMTriggerID, FSMStateID>();
            triggers = new List<BaseTrigger>();
        }

        public abstract void Init();
        
        /// <summary>
        /// ���ӳ���
        /// </summary>
        /// <param name="triggerID"></param>
        /// <param name="stateID"></param>
        public void AddMap(FSMTriggerID triggerID,FSMStateID stateID)
        {
            map.Add(triggerID, stateID);

            BaseTrigger trigger = Activator.CreateInstance(Type.GetType("AI." + triggerID + "Trigger")) as BaseTrigger;
            triggers.Add(trigger);
        }

        public virtual void EnterState(BaseFSM fsm) { }
        public virtual void ActionState(BaseFSM fsm) { }
        public virtual void ExitState(BaseFSM fsm) { }
    }
}