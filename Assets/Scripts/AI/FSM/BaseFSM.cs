using System;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System.Collections;

namespace AI
{
    //TODO:武需分类，玩家 AI，类型
    public enum EnemyWeaponType
    {
        Bow = 4,
        Wand = 5,
        RedMouse = 102
    }

    public class BaseFSM : MonoBehaviour,IBeDamage
    {
        //AI数据，房间管理器生成AI时赋值
        public EnemyData data;

        public Animator anim;
        public Transform target; //目标
        protected AStartAgent agent;
        //巡逻路点  
        public Vector3[] wayPoint;
        protected List<BaseState> states;
        protected BaseState defaultState;
        protected BaseState currentState;
        //外部测试用
        public FSMStateID testStateID;
        protected FSMStateID defaultStateID;
        public IAlive targetAlive;
        //是否找到目标
        public bool findTarget;
        //检测目标间隔时间
        protected float detectionTimer;

        protected WeaponAttack attack;
        protected SpriteRenderer sprite;

        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            agent = CodeHelper.AddOrGetComponent<AStartAgent>(gameObject);
            target = PlayerController.instance.transform;
            targetAlive = target.GetComponent<IAlive>();
            attack = transform.GetComponentInChildren<WeaponAttack>();
            sprite = GetComponent<SpriteRenderer>();
            
        }

        public void EnterCurrentState()
        {
            if (currentState != null)
            {
                currentState.EnterState(this);
            }
        }

        protected virtual void Start()
        {  
            ConfigFSM(data.configurationFile);
            InitDefaultState();
            InitWeaponData();
        }

        protected virtual void Update()
        {
            //固定时间内检测
            if (detectionTimer > data.detectionInterval)
            {
                DetectionTarget();
                detectionTimer = 0;
            }
            detectionTimer += Time.deltaTime;
            //判断当前状态转换条件是否满足
            currentState.Reason(this);
            //执行当前状态
            currentState.ActionState(this);
            dist = Vector3.Distance(transform.position,target.position);
        }

        public float dist;

        protected virtual void DetectionTarget()
        {
            if (targetAlive.IsAlive)
            {
                findTarget = SelectTarget.FindTargets(transform, target, data.sightDist, data.sightAngle);
            }
        }

        #region 状态机
        //配置状态机
        public void ConfigFSM(string fileName)
        {
            states = new List<BaseState>();
            //得到映射表
            Dictionary<string, Dictionary<string, string>> map = LoadGameData.LoadAIMap(fileName);
            foreach (var mainKey in map)
            {
                //获取状态类型，加入状态列表
                BaseState state = Activator.CreateInstance(Type.GetType("AI." + mainKey.Key + "State")) as BaseState;
                states.Add(state);

                foreach (var subKey in mainKey.Value)
                {
                    //获取条件状态映射
                    FSMTriggerID triggerID = (FSMTriggerID)Enum.Parse(typeof(FSMTriggerID), subKey.Key);
                    FSMStateID stateID = (FSMStateID)Enum.Parse(typeof(FSMStateID), subKey.Value);
                    state.AddMap(triggerID, stateID);
                }
            }
        }

        public void ChangeState(FSMStateID stateID)
        {
            //离开当前状态
            currentState.ExitState(this);
            //切换并进入
            currentState = stateID == FSMStateID.Default ? defaultState : states.Find(s => s.stateID == stateID);
            currentState.EnterState(this);
            testStateID = currentState.stateID;
        }

        protected void InitDefaultState()
        {
            //默认状态
            defaultStateID = FSMStateID.Patrol;
            currentState = defaultState = states.Find(s => s.stateID == defaultStateID);
            currentState.EnterState(this);
            testStateID = currentState.stateID;
        }
        #endregion
     
        public virtual void MoveToTarget(Vector3 targetPos,float speed)
        {
            //TODO:各个点位之间的判断，目前只判断了与终点的位置关系
            FaceToTarget(targetPos);
            
            agent.MoveToTarget(targetPos, speed);
        }

        public void FaceToTarget(Vector3 targetPos)
        {
            //判断转向
            Vector3 cross = Vector3.Cross(transform.up, (transform.position - targetPos).normalized);
            if (cross.z < 0)
            {
                ChangeFace(Face.Left);
            }
            else if (cross.z > 0)
            {
                ChangeFace(Face.Right);
            }
        }

        public void ChangeFace(Face face)
        {
            switch (face)
            {
                case Face.Right:
                    //transform.localScale = new Vector3(-1, 1, 1);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Face.Left:
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
            }
        }
     
        public void StopMove()
        {
            agent.StopFinding();
        }
        //TODO: 优化
        protected void InitWeaponData()
        {
            WeaponData wp = LoadGameData.GetWeaponData(data.wpID);
            Transform weaponPoint = CodeHelper.FindChild(transform, "Weapon");
            GameObject prefab = Resources.Load<GameObject>(wp.weaponName);
            GameObject go = GameObjectPool.instance.CreateObject(wp.weaponName, prefab,
                weaponPoint.position, Quaternion.identity
                );
            go.transform.SetParent(weaponPoint);
            wp.firePoint = CodeHelper.FindChild(go.transform, "FirePoint");
            wp.obj = go;
            attack = go.GetComponent<WeaponAttack>();
            attack.Data = wp;
            wp.owner = transform;
        }

        public virtual void Attack()
        {
            attack.Attack();
        }

        public void BeDamge(int damage)
        {
            if (damage <= 0)
                return;

            data.hp -= damage;

            StartCoroutine(BeDamageHandle());
        }

        protected IEnumerator BeDamageHandle()
        {
            sprite.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            sprite.color = new Color(1, 1, 1, 1);
        }
    }
}