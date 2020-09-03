using System;
using UnityEngine;

namespace AI.FSM
{
    public class AttackingState : FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Attacking;
        }

        private float attackTime=0;
        public override void Action(BaseFSM fsm)
        {
            if (attackTime > fsm.chStatus.attackSpeed)
            {
                
                attackTime = 0;
                fsm.attackCollider.enabled = true;
                fsm.PlayAnimation(fsm.animParams.Attack);
            }
            else
            {
                fsm.attackCollider.enabled = false;
                attackTime += Time.deltaTime;
                fsm.PlayAnimation(fsm.animParams.Idle);
            }
            fsm.transform.LookAt(fsm.targetObject);
        }

        public override void EnterState(BaseFSM fsm)
        {
            fsm.StopMove();
            fsm.PlayAnimation(fsm.animParams.Idle);
        }
        public override void ExitState(BaseFSM fsm)
        {
            base.ExitState(fsm);
        }
    }
}