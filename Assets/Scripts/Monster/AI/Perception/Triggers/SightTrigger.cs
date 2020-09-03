using System;
using UnityEngine;

namespace AI.Perception
{
    public class SightTrigger :AbstractTrigger
    {
        public Transform recievePos;

        public override void Init()
        {
            if (recievePos == null)
            {
                recievePos = transform;
            }
            triggerType = TriggerType.Sight;
        }

        protected override void Start()
        {
            base.Start();
            recievePos = this.gameObject.transform;
        }
    }
}