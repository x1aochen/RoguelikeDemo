using System;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System.Collections;

namespace AI
{
    //TODO:������࣬��� AI������
    public enum EnemyWeaponType
    {
        Bow = 4,
        Wand = 5,
        RedMouse = 102
    }

    public class BaseFSM : MonoBehaviour,IBeDamage
    {
        //AI���ݣ��������������AIʱ��ֵ
        public EnemyData data;

        public Animator anim;
        public Transform target; //Ŀ��
        protected AStartAgent agent;
        //Ѳ��·��  
        public Vector3[] wayPoint;
        protected List<BaseState> states;
        protected BaseState defaultState;
        protected BaseState currentState;
        //�ⲿ������
        public FSMStateID testStateID;
        protected FSMStateID defaultStateID;
        public IAlive targetAlive;
        //�Ƿ��ҵ�Ŀ��
        public bool findTarget;
        //���Ŀ����ʱ��
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
            //�̶�ʱ���ڼ��
            if (detectionTimer > data.detectionInterval)
            {
                DetectionTarget();
                detectionTimer = 0;
            }
            detectionTimer += Time.deltaTime;
            //�жϵ�ǰ״̬ת�������Ƿ�����
            currentState.Reason(this);
            //ִ�е�ǰ״̬
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

        #region ״̬��
        //����״̬��
        public void ConfigFSM(string fileName)
        {
            states = new List<BaseState>();
            //�õ�ӳ���
            Dictionary<string, Dictionary<string, string>> map = LoadGameData.LoadAIMap(fileName);
            foreach (var mainKey in map)
            {
                //��ȡ״̬���ͣ�����״̬�б�
                BaseState state = Activator.CreateInstance(Type.GetType("AI." + mainKey.Key + "State")) as BaseState;
                states.Add(state);

                foreach (var subKey in mainKey.Value)
                {
                    //��ȡ����״̬ӳ��
                    FSMTriggerID triggerID = (FSMTriggerID)Enum.Parse(typeof(FSMTriggerID), subKey.Key);
                    FSMStateID stateID = (FSMStateID)Enum.Parse(typeof(FSMStateID), subKey.Value);
                    state.AddMap(triggerID, stateID);
                }
            }
        }

        public void ChangeState(FSMStateID stateID)
        {
            //�뿪��ǰ״̬
            currentState.ExitState(this);
            //�л�������
            currentState = stateID == FSMStateID.Default ? defaultState : states.Find(s => s.stateID == stateID);
            currentState.EnterState(this);
            testStateID = currentState.stateID;
        }

        protected void InitDefaultState()
        {
            //Ĭ��״̬
            defaultStateID = FSMStateID.Patrol;
            currentState = defaultState = states.Find(s => s.stateID == defaultStateID);
            currentState.EnterState(this);
            testStateID = currentState.stateID;
        }
        #endregion
     
        public virtual void MoveToTarget(Vector3 targetPos,float speed)
        {
            //TODO:������λ֮����жϣ�Ŀǰֻ�ж������յ��λ�ù�ϵ
            FaceToTarget(targetPos);
            
            agent.MoveToTarget(targetPos, speed);
        }

        public void FaceToTarget(Vector3 targetPos)
        {
            //�ж�ת��
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
        //TODO: �Ż�
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