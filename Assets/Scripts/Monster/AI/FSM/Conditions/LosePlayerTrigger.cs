using System;
using UnityEngine;
using FPSProject.Character;

namespace AI.FSM
{


    public class LosePlayerTrigger : FSMTrigger
    {
        public override void Init()
        {
            triggerid = FSMTriggerID.LosePlayer;
        }

        public override bool HandleTrigger(BaseFSM fsm)
        {
            if (fsm.targetObject != null)
            {
                if (Vector3.Distance(fsm.targetObject.position, fsm.transform.position) > fsm.sightDistance)
                {
                    fsm.targetObject = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
