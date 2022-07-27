using System.Collections.Generic;
using UnityEngine;
using UI;
using Player;
using System;
using AI;
using System.Collections;

namespace Room
{
    public class RoomManager : MonoSingleton<RoomManager>
    {
        private List<Room> rooms;
        private Transform player;
        private TransitionPanel transition;
        private int currentRoom = -1;
        private int preRoom;
        private Vector3 targetPos;
        private AI.Grid myGrid;
        private Vector2Int gridSize;

        public override void Init()
        {
            player = PlayerController.instance.transform;
            transition = new TransitionPanel();

            //�����з�����뷿���б���
            GameObject room = GameObject.Find(Consts.Room);
            Room[] tmp = room.GetComponentsInChildren<Room>();
            rooms = new List<Room>(tmp.Length);
            rooms.AddRange(tmp); //TODO: AddRange���ִ�У����ݼ��Σ�һ�����Զ����ݵ������С��
            //rooms.AddRange(room.GetComponentsInChildren<Room>());

            //��Ϸ��ʼʱ�����õķ����Ƚ��ã���Ҫ��תʱ�ٿ���
            for (int i = 1; i < rooms.Count; i++)
            { 
                rooms[i].gameObject.SetActive(false);
            }

            EventManager.instance.AddListener(EventName.Transition, EndFadeIn);

            myGrid = AI.Grid.instance;
        }

        private void Start()
        {
             JumpRoom(0);
        }
        public void JumpRoom(int roomIndex)
        {
            if (roomIndex >= rooms.Count || roomIndex < 0)
                return;

            //������һ������
            rooms[roomIndex].gameObject.SetActive(true);


            gridSize = myGrid.GenerateGrid(roomIndex);
            //������ת����ת���ϲ�Ľ�����
            if (roomIndex < currentRoom)
            {
                targetPos = rooms[roomIndex].endPortal.position;
            }
            //������ת����ת���²����ʼ��
            else if (roomIndex > currentRoom)
            {
                targetPos = rooms[roomIndex].startPortal.position;
            }

            preRoom = currentRoom;
            currentRoom = roomIndex;

            //����������壬���뵭��Ч��������δ�����ܼ����������
            UIManager.instance.Push(transition);
            transition.StartFadeIn();


            GenerateEnemies();

        }

        private void GenerateEnemies()
        {
            for (int i = 0; i < rooms[currentRoom].enemyCount; i++)
            { 
                GenerateEnemy();
            }
        }

        
        /// <summary>
        /// �������һ������
        /// </summary>
        private void GenerateEnemy()
        {
            //ѡȡ���ɵ�������
            //��ȡ��ǰ��������ɵ��˵���������
            //��ȡ��ǰ���ɵ�������
            int index = CodeHelper.Random(0, rooms[currentRoom].enemies.Length);
            EnemyData data = LoadGameData.GetEnemyData((int)rooms[currentRoom].enemies[index]) as EnemyData;

            int x, y;
            //ѡȡ�����
            while (true)
            {
                x = CodeHelper.Random(0, gridSize.x);
                y = CodeHelper.Random(0, gridSize.y);

                if (myGrid.GridWalkable(x,y))
                    break;
            }
            //ʵ��������
            GameObject go = GameObjectPool.instance.CreateObject(data.type.ToString(), data.prefab,
                myGrid.WorldPosFromNode(x,y), Quaternion.identity
                );
            //״̬����Ա��ʼ��
            BaseFSM fsm = CodeHelper.AddOrGetComponent<BaseFSM>(go);
            //�������·��
            Vector3[] wayPoints = new Vector3[3];
            for (int i = 0;i < 3; i++)
            {
                while (true)
                {
                    x = CodeHelper.Random(0, gridSize.x);
                    y = CodeHelper.Random(0, gridSize.y);
                    if (myGrid.GridWalkable(x, y))
                        break;
                }

                wayPoints[i] = myGrid.WorldPosFromNode(x, y);
            }

            fsm.data = data;
            fsm.wayPoint = wayPoints;
            fsm.EnterCurrentState();
        }

        private void EnemyCountDecrease()
        {
            rooms[currentRoom].currentEnemyCount--;
            //TODO: ����������
        }


        //��������
        //ִ��ǰ�̷�����ã�����������������λ��
        private void EndFadeIn(EventArgs args)
        {
            //������һ������
            if (preRoom != -1)
                rooms[preRoom].gameObject.SetActive(false);
            player.position = targetPos;
            Camera.main.transform.position = new Vector3(targetPos.x, targetPos.y, Camera.main.transform.position.z);
            transition.StartFadeOut();
        }
    }
}