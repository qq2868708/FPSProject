using UnityEngine;
using System.Collections;


namespace AI.Perception
{
    public abstract class AbstractTrigger:MonoBehaviour
    {
        public bool isRemove;
        public TriggerType triggerType;

        protected virtual void Start()
        {
            SensorTriggerSystem sys = SensorTriggerSystem.instance;
            sys.AddTrigger(this);
        }
        abstract public void Init();

        public void OnDestroy()
        {
            isRemove = true;
        }
    }
}