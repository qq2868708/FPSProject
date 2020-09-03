using System;

namespace AI.FSM
{


    public class NoHealthTrigger :FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.NoHealth;
        }

        public override bool HandleTrigger(BaseFSM fsm)
        {
            return fsm.chStatus.currentHp <= 0;
        }
    }
}
