using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSProject.Character;
using UnityEngine;
using UnityEngine.AI;
using AI.Perception;

namespace AI.FSM
{
    [Serializable]
    public class BaseFSM:MonoBehaviour
    {
        #region 1.0
        //字段
        private CharacterAnimation chAnim;

        private FSMState currentState;
        public FSMStateID currentStateID;

        private FSMState defaultState;
        public FSMStateID defaultStateID;

        public SphereCollider attackCollider;

        public LevelManager instance;

        //状态库，所有可能的状态，所以状态机的工作就是切换状态，然后执行每个状态内的条件判断，按照
        //状态内的条件-状态对照表获得相应的对照，然后回到这里查找对应的状态并切换
        public List<FSMState> states=new List<FSMState>();

        //方法
        public void ChangActiveState(FSMTriggerID triggerid)
        {
            var nextStateID = currentState.GetOutputState(triggerid);
            if (nextStateID == FSMStateID.None)
            {
                return;
            }
            //如果是默认状态
            FSMState nextstate = null;
            if (nextStateID == FSMStateID.Default)
            {
                nextstate = defaultState;
            }
            else
            {
                nextstate = states.Find(s => s.stateid == nextStateID);
            }

            currentState.ExitState(this);
            currentState = nextstate;
            currentStateID = currentState.stateid;
            currentState.EnterState(this);
            
        }
        #endregion

        #region 2.0
        public string aiConfigFile="AI_01.txt";//AI配置文件，定制状态转换表
        public AnimationParams animParams;
        public CharacterStatus chStatus;

        private void Awake()
        {
            ConfigFSM();
        }

        private void ConfigFSM()
        {
            #region 配置文件
            //调用AI配置文件
            var dic = AIConfigurationReader.Load(aiConfigFile);
            foreach (var stateName in dic.Keys)
            {
                Type typeObj = Type.GetType("AI.FSM." + stateName + "State");
                if (typeObj == null)
                {
                    Debug.Log(stateName);
                    continue;
                }
                var stateObj = Activator.CreateInstance(typeObj) as FSMState;

                foreach(var triggerid in dic[stateName].Keys)
                {
                    //string->enum
                    var trigger = (FSMTriggerID)(Enum.Parse(typeof(FSMTriggerID), triggerid));
                    var state = (FSMStateID)(Enum.Parse(typeof(FSMStateID), dic[stateName][triggerid]));
                    stateObj.AddTrigger(trigger, state);
                }
                states.Add(stateObj);
            }


            #endregion
        }

        private void InitDefaultState()
        {
            defaultState = states.Find(s => s.stateid == defaultStateID);
            currentState = defaultState;
            currentStateID = defaultStateID;
        }

        private void OnEnable()
        {
            InitDefaultState();//执行的时机和执行的评率
        }

        public void PlayAnimation(string animPara)
        {
            chAnim.PlayAnimation(animPara);
        }

        public void Start()
        {
            chStatus = GetComponentInChildren<CharacterStatus>();
            chAnim = GetComponentInChildren<CharacterAnimation>();
            navAgent = GetComponent<NavMeshAgent>();
            
            sightSensor = GetComponent<SightSensor>();
            if (sightSensor!= null)
            {
                sightSensor.OnPerception += sightSensor_OnPerception;
                sightSensor.OnNonPerception += sightSensor_OnNonPerception;
            }

            attackCollider = GetComponentInChildren<SphereCollider>();

            instance = LevelManager.instance;
        }

        private void Update()
        {
            //实时检查条件
            currentState.Reason(this);
            currentState.Action(this);
        }
        #endregion

        #region 3.0
        public string[] targetTags = { "Player" };
        public Transform targetObject=null;
        public float sightDistance=10;
        public float moveSpeed = 1;
        private NavMeshAgent navAgent;

        public void MoveToTarget(Vector3 pos,float speed,float stopDistance)
        {
            navAgent.speed = speed;
            navAgent.stoppingDistance = stopDistance;
            navAgent.SetDestination(pos);
        }

        public void StopMove()
        {
            navAgent.enabled = false;
            navAgent.enabled = true;
        }

        private void OnDisable()
        {
            if (currentStateID != FSMStateID.Dead)
            {
                currentState.ExitState(this);
                currentState = states.Find(p => p.stateid == FSMStateID.Idle);
                currentStateID = currentState.stateid;
                PlayAnimation(animParams.Idle);

            }
            else
            {
                var sensors = GetComponents<AbstractSensor>();
                foreach(var item in sensors)
                {
                    item.enabled = false;
                }
            }
        }
        #endregion


        #region 5.0
        public Transform[] wayPoints;

        public float patrolArrivalDistance = 1;

        public float walkSpeed = 3;

        public bool isPatrolComplete = false;

        public PatrolMode patrolMode = PatrolMode.Once;
        #endregion

        #region 6.0 智能感应器
        private SightSensor sightSensor;
        public void sightSensor_OnPerception(List<AbstractTrigger> listTrigger)
        {
            targetObject = null;
            var tempList = listTrigger.FindAll(p=>Array.IndexOf(targetTags,p.tag)>=0);
            if (tempList.Count > 0)
            {
                tempList = tempList.FindAll(p => p.GetComponent<CharacterStatus>().currentHp > 0);
                if (tempList.Count > 0)
                {
                    targetObject = ArrayHelper.Min(tempList.ToArray(), p => Vector3.Distance(p.transform.position, transform.position)).transform;
                }
            }
        }

        public void sightSensor_OnNonPerception()
        {
            Debug.Log("meikanjian");
        }
        #endregion

        public void m_Debug(string s)
        {
            Debug.Log(s);
        }
    }
}
