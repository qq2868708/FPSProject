using System;
using UnityEngine;

namespace AI.FSM
{
    public class PursuitState : FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Pursuit;
        }

        public override void Action(BaseFSM fsm)
        {
            if (fsm.targetObject == null)
            {
                return;
            }
            fsm.PlayAnimation(fsm.animParams.Run);

            fsm.MoveToTarget(fsm.targetObject.position, fsm.moveSpeed, fsm.chStatus.attackDistance);
        }

        public override void ExitState(BaseFSM fsm)
        {
            base.ExitState(fsm);
        }
    }
}