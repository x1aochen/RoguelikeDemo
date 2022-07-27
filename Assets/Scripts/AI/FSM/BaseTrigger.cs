using UnityEngine;

namespace AI
{
    public abstract class BaseTrigger
    {
        public FSMTriggerID triggerID { get; set; }

        public BaseTrigger()
        {
            Init();
        }
        /// <summary>
        /// �߼��ж��Ƿ��ת������
        /// </summary>
        /// <param name="fsm"></param>
        /// <returns></returns>
        public abstract bool HandleTrigger(BaseFSM fsm);

        public abstract void Init();

    }
}