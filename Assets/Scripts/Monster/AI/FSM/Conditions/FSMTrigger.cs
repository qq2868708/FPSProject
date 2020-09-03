using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    abstract public class FSMTrigger
    {
        //字段
        public FSMTriggerID triggerid;
        //方法
        public FSMTrigger()
        {
            Init();
        }
        //初始化
        abstract public void Init();
        abstract public bool HandleTrigger(BaseFSM fsm);       
    }
}
