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

            //将所有房间加入房间列表中
            GameObject room = GameObject.Find(Consts.Room);
            Room[] tmp = room.GetComponentsInChildren<Room>();
            rooms = new List<Room>(tmp.Length);
            rooms.AddRange(tmp); //TODO: AddRange如何执行？扩容几次？一次性自动扩容到所需大小吗？
            //rooms.AddRange(room.GetComponentsInChildren<Room>());

            //游戏开始时，无用的房间先禁用，需要跳转时再开启
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

            //开启下一个房间
            rooms[roomIndex].gameObject.SetActive(true);


            gridSize = myGrid.GenerateGrid(roomIndex);
            //向上跳转，跳转至上层的结束点
            if (roomIndex < currentRoom)
            {
                targetPos = rooms[roomIndex].endPortal.position;
            }
            //向下跳转，跳转至下层的起始点
            else if (roomIndex > currentRoom)
            {
                targetPos = rooms[roomIndex].startPortal.position;
            }

            preRoom = currentRoom;
            currentRoom = roomIndex;

            //开启过场面板，淡入淡出效果，或者未来可能加入的新需求
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
        /// 随机生成一个敌人
        /// </summary>
        private void GenerateEnemy()
        {
            //选取生成敌人类型
            //获取当前房间可生成敌人的种类数量
            //获取当前生成敌人类型
            int index = CodeHelper.Random(0, rooms[currentRoom].enemies.Length);
            EnemyData data = LoadGameData.GetEnemyData((int)rooms[currentRoom].enemies[index]) as EnemyData;

            int x, y;
            //选取随机点
            while (true)
            {
                x = CodeHelper.Random(0, gridSize.x);
                y = CodeHelper.Random(0, gridSize.y);

                if (myGrid.GridWalkable(x,y))
                    break;
            }
            //实例化敌人
            GameObject go = GameObjectPool.instance.CreateObject(data.type.ToString(), data.prefab,
                myGrid.WorldPosFromNode(x,y), Quaternion.identity
                );
            //状态机成员初始化
            BaseFSM fsm = CodeHelper.AddOrGetComponent<BaseFSM>(go);
            //随机设置路点
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
            //TODO: 开启传送门
        }


        //结束淡入
        //执行前继房间禁用，淡出，玩家与摄像机位置
        private void EndFadeIn(EventArgs args)
        {
            //隐藏上一个房间
            if (preRoom != -1)
                rooms[preRoom].gameObject.SetActive(false);
            player.position = targetPos;
            Camera.main.transform.position = new Vector3(targetPos.x, targetPos.y, Camera.main.transform.position.z);
            transition.StartFadeOut();
        }
    }
}