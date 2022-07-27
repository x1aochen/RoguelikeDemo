using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Room
{
    public class Room : MonoBehaviour
    {
        protected int roomIndex;
        [HideInInspector]
        public Transform startPortal;
        [HideInInspector]
        public Transform endPortal;
        //当前房间可生成的敌人类型
        [Header("----------当前房间可生成的敌人类型------------")]
        public EnemyType[] enemies;

        [Tooltip("房间敌人总量")]
        public int enemyCount;
        [Tooltip("当前敌人数量")]
        public int currentEnemyCount;
        protected virtual void Awake()
        {
            AddWallCollider();
            currentEnemyCount = enemyCount;
            //上层传送门
            startPortal = CodeHelper.FindChild(transform, Consts.PrePortal);
            if (startPortal != null)
            {
                Portal p = startPortal.gameObject.AddComponent<Portal>();
                p.JumpIndex = roomIndex - 1;
            }

            //下层传送门
            endPortal = CodeHelper.FindChild(transform, Consts.NextPortal);
            if (endPortal != null)
            {
                Portal p = endPortal.gameObject.AddComponent<Portal>();
                p.JumpIndex = roomIndex + 1;
            }
        }

        /// <summary>
        /// 给墙面添加物理碰撞
        /// </summary>
        protected void AddWallCollider()
        {
            GameObject wall = CodeHelper.FindChild(transform, Consts.Wall).gameObject;
            CodeHelper.AddOrGetComponent<TilemapCollider2D>(wall).usedByComposite = true;
            CodeHelper.AddOrGetComponent<CompositeCollider2D>(wall);
            CodeHelper.AddOrGetComponent<Rigidbody2D>(wall).bodyType = RigidbodyType2D.Kinematic;
        }

    }
}






