using System;
using UnityEngine;
using FPSProject.Character;

namespace AI.FSM
{
    public class CompletePatrolTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.CompletePatrol;
        }

        public override bool HandleTrigger(BaseFSM fsm)
        {
            if (fsm.isPatrolComplete == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
