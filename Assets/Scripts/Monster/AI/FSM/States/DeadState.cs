using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.FSM
{
    public class DeadState : FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Dead;
        }

        public override void Action(BaseFSM fsm)
        {
            
        }

        public override void EnterState(BaseFSM fsm)
        {
            fsm.PlayAnimation(fsm.animParams.Dead);
            fsm.StopMove();
            fsm.enabled = false;
            fsm.instance.CheckVictory();
        }
    }
}