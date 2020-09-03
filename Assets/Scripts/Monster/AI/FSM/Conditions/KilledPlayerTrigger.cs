using System;
using UnityEngine;
using FPSProject.Character;

namespace AI.FSM
{
    public class KilledPlayerTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.KilledPlayer;
        }

        public override bool HandleTrigger(BaseFSM fsm)
        {
            if (fsm.targetObject.GetComponent<CharacterStatus>().currentHp <=0)
            {
                fsm.targetObject = null;
                return true;
            }
            return false;
        }
    }
}
