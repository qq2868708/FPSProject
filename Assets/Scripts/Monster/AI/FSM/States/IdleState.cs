using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.FSM
{
    public class IdleState: FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Idle;
        }

        public override void Action(BaseFSM fsm)
        {
            fsm.PlayAnimation(fsm.animParams.Idle);
        }

        public override void EnterState(BaseFSM fsm)
        {
            base.EnterState(fsm);
            fsm.GetComponent<NavMeshAgent>().enabled = false;
            fsm.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}