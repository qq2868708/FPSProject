using System;
using UnityEngine;

namespace AI.FSM
{
    public class WithOutAttackRangeTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.WithOutAttackRange;
        }

        public override bool HandleTrigger(BaseFSM fsm)
        {
            
            if (fsm.targetObject != null)
            {
                float dis = Vector3.Distance(fsm.transform.position, fsm.targetObject.position);
                if(dis>fsm.chStatus.attackDistance&&dis<fsm.sightDistance)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
