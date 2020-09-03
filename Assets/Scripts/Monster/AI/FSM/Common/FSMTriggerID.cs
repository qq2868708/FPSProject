using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    /// <summary>
    /// 状态转换条件
    /// </summary>
    public enum FSMTriggerID
    {
        /// <summary>
        /// 生命为0
        /// </summary>	
        NoHealth,
        //发现目标	
        SawPlayer,
        //目标进入攻击范围	
        ReachPlayer,
        //丢失玩家	
        LosePlayer,
        //完成巡逻	
        CompletePatrol,
        //打死目标	
        KilledPlayer,
        //目标不在攻击范围
        //玩家离开攻击范围	
        WithOutAttackRange,
    }
}
