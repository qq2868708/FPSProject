using System;
using UnityEngine;

namespace AI.FSM
{
    public class PatrollingState : FSMState
    {
        public override void Init()
        {
            stateid = FSMStateID.Patrolling;
        }

        private int currentWayPoint;
        public override void Action(BaseFSM fsm)
        {
            if (Vector3.Distance(fsm.transform.position, fsm.wayPoints[currentWayPoint].position) < fsm.patrolArrivalDistance)
            {
                if (currentWayPoint == fsm.wayPoints.Length - 1)
                {
                    switch (fsm.patrolMode)
                    {
                        case PatrolMode.Once:
                            fsm.isPatrolComplete = true;
                            return;
                        case PatrolMode.PingPong:
                            Array.Reverse(fsm.wayPoints);
                            currentWayPoint += 1;
                            break;
                    }
                }

                currentWayPoint = (currentWayPoint + 1) % fsm.wayPoints.Length;
            }
            fsm.MoveToTarget(fsm.wayPoints[currentWayPoint].position, fsm.walkSpeed, fsm.patrolArrivalDistance);
            fsm.PlayAnimation(fsm.animParams.Walk);
        }

        public override void EnterState(BaseFSM fsm)
        {
            fsm.isPatrolComplete = false;
        }
        public override void ExitState(BaseFSM fsm)
        {
            fsm.StopMove();
        }
    }
}