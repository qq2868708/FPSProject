using System;
using UnityEngine;

namespace AI.FSM
{


    public class ReachPlayerTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.ReachPlayer;
        }

        public override bool HandleTrigger(BaseFSM fsm)
        {
            if (fsm.targetObject != null)
            {
                bool b = Vector3.Distance(fsm.transform.position, fsm.targetObject.position) < fsm.chStatus.attackDistance-0.5;
                return b;
            }
            return false;
        }
    }
}
