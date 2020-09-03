using UnityEngine;
using System.Collections;

namespace AI.Perception
{
    public class SightSensor : AbstractSensor
    {
        public float sightDistance;
        public float sightAngle;

        public bool enableAngel;
        public bool enableRay;

        public Transform sendPos;

        public override void Init()
        {
            if (sendPos == null)
            {
                sendPos = transform;
            }
        }

        //查找合规的触发器
        protected override bool TestTrigger(AbstractTrigger trigger)
        {
            if(trigger.triggerType!=TriggerType.Sight)
            {
                return false;
            }
            var temTrigger = trigger as SightTrigger;

            var dir = temTrigger.recievePos.position - sendPos.position;
            var result = dir.magnitude < sightDistance;
            if (enableAngel)
            {
                bool b1 = Vector3.Angle(transform.forward, dir) < sightAngle / 2;
                result = result && b1;
            }

            if (enableRay)
            {
                RaycastHit hit;
                bool b1 = Physics.Raycast(sendPos.position, dir, out hit, sightDistance)&& hit.collider.gameObject==trigger.gameObject;
                result = result && b1;
            }

            return result;
        }
        
    }
}