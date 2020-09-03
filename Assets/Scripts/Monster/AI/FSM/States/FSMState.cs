using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    /// <summary>
    /// 状态抽象类
    /// </summary>
    abstract  public  class FSMState
    {
       //字段************
       //状态编号
       public FSMStateID stateid;
       //条件列表：储存所有条件对象，用于执行方法
       private List<FSMTrigger> triggers = new List<FSMTrigger>();
       //转换映射表:从当前state装换到其他state的条件，仅仅是枚举类型的字典，不提供方法，只提供名字
       private Dictionary<FSMTriggerID, FSMStateID> map = 
           new Dictionary<FSMTriggerID, FSMStateID>();
       //方法*************
       public FSMState()
       {
           Init();
       }
       //1初始化 
      abstract public void Init();
       //2添加条件 :条件和状态的 映射=对应关系
       public void AddTrigger(FSMTriggerID triggerid,FSMStateID stateid)
       {
           if (map.ContainsKey(triggerid))
           {
               map[triggerid] = stateid;
           }
           else
           {
               map.Add(triggerid, stateid);
               AddTriggerObject(triggerid);
           }
       }
        //添加条件对象
       private void AddTriggerObject(FSMTriggerID triggerid)
       {
           Type type = Type.GetType("AI.FSM." + triggerid+ "Trigger");
           if (type != null)
           {
               var triggerObj = Activator.CreateInstance(type) as FSMTrigger;
               triggers.Add(triggerObj);
           }
       }
       //3删除条件 
       public void RemoveTrigger(FSMTriggerID triggerid)
       {
           if (map.ContainsKey(triggerid))
           {
               map.Remove(triggerid);
               RemoveTriggerObject(triggerid);
           }
       }
        //删除条件对象
       private void RemoveTriggerObject(FSMTriggerID triggerid)
       {
           triggers.RemoveAll(t => t.triggerid == triggerid);   
       }
       //4查找映射=查找条件映射
       public FSMStateID GetOutputState(FSMTriggerID triggerid)
       {
           if (map.ContainsKey(triggerid))
           {
                var a = map[triggerid];
               return a;
           }
           return FSMStateID.None;       
       }
       //5状态行为 -每个状态行为不同
       abstract public void Action(BaseFSM fsm);       
       //6条件检测 -大部分状态类似，也有不同
       virtual public void Reason(BaseFSM fsm)
       {
           for (int i = 0; i < triggers.Count;i++)
           {
                //fsm.m_Debug(Enum.GetName(typeof(FSMStateID), stateid));
                //fsm.m_Debug(Enum.GetName(typeof(FSMTriggerID), triggers[i].triggerid));
                if (triggers[i].HandleTrigger(fsm))
               {
                   fsm.ChangActiveState(triggers[i].triggerid);
                   return;
               }
           }       
       }
       //7离开状态 
       virtual  public void ExitState(BaseFSM fsm)
       { }
       //8进入状态
       virtual public void EnterState(BaseFSM fsm)
       { }
    }
}
