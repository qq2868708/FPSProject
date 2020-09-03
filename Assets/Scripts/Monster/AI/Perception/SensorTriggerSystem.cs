using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AI.Perception
{
    public class SensorTriggerSystem:Mono_Singleton<SensorTriggerSystem>
    {
        public float checkInterval=0.2f;

        public List<AbstractSensor> listSensor=new List<AbstractSensor>();
        public List<AbstractSensor> m_listSensor = new List<AbstractSensor>();

        public List<AbstractTrigger> listTrigger=new List<AbstractTrigger>();
        public List<AbstractTrigger> m_listTrigger = new List<AbstractTrigger>();

        private void CheckTrigger()
        {
            //对每一个需要检测的感应器进行符合条件的触发器的筛选
            for (int i = 0; i < m_listSensor.Count; i++)
            {
                if (m_listSensor[i].enabled)
                {
                    m_listSensor[i].OnTestTrigger(m_listTrigger);
                }
            }
        }

        private void UpadateSystem()
        {
            //这段代码导致isremove不能用，一旦关闭将不会被重新加入列表，没有相应的方法
            m_listSensor = listSensor.FindAll(s => !s.isRemove);
            m_listTrigger = listTrigger.FindAll(t => !t.isRemove);
        }

        private void OnCheck()
        {
            UpadateSystem();
            CheckTrigger();
        }

        private void OnEnable()
        {
            InvokeRepeating("OnCheck", 0, checkInterval);
        }

        private void OnDisable()
        {
            CancelInvoke("OnCheck");
        }

        public void AddSensor(AbstractSensor sensor)
        {
            listSensor.Add(sensor);
        }

        public void AddTrigger(AbstractTrigger trigger)
        {
            listTrigger.Add(trigger);
        }
    }
}