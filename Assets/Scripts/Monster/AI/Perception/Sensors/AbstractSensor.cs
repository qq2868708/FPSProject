using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace AI.Perception
{
    public abstract class AbstractSensor:MonoBehaviour
    {
        public bool isRemove;

        private void Start()
        {
            Init();
            SensorTriggerSystem sys = SensorTriggerSystem.instance;
            sys.AddSensor(this);
        }

        abstract public void Init();

        private void OnDestroy()
        {

        }

        public void OnTestTrigger(List<AbstractTrigger> listTriggers)
        {
            //筛选出符合要求的触发器：有相应的触发器的（没有禁用），不是自己，符合检测条件的（如视距和视角）
            listTriggers = listTriggers.FindAll(t => t.enabled&&t.gameObject!=this.gameObject&&TestTrigger(t));
            if (listTriggers.Count > 0)
            {
                if (OnPerception != null)
                {
                    OnPerception(listTriggers);
                }
            }
            else
            {
                if (OnNonPerception != null)
                {
                    OnNonPerception();
                }
            }
        }

        /// <summary>
        /// 检测触发器是否被感知
        /// </summary>
        /// <param name="trigger"></param>
        abstract protected bool TestTrigger(AbstractTrigger trigger);

        
        public event Action OnNonPerception;

        public event Action<List<AbstractTrigger>> OnPerception;
    }
}