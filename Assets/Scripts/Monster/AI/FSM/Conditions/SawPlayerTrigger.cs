using System;

namespace AI.FSM
{


    public class SawPlayerTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.SawPlayer;
        }

        public override bool HandleTrigger(BaseFSM fsm)
        {
            bool b = false;
            if (fsm.targetObject != null)
            {
                b = true;
            }
            return b;
        }
    }
}
