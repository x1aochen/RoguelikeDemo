namespace AI
{
    public enum FSMTriggerID
    {
        /// <summary>
        /// 生命为0
        /// </summary>
        Nohealth,
        /// <summary>
        /// 发现目标
        /// </summary>
        SawTarget,
        /// <summary>
        /// 丢失目标
        /// </summary>
        LoseTarget,
        /// <summary>
        /// 到达目标
        /// </summary>
        ReachTarget,
        /// <summary>
        /// 丢失攻击范围
        /// </summary>
        WithoutAttackRange,
        /// <summary>
        /// 杀死目标
        /// </summary>
        KilledTarget
    }
}