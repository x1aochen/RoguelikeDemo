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
        //��ǰ��������ɵĵ�������
        [Header("----------��ǰ��������ɵĵ�������------------")]
        public EnemyType[] enemies;

        [Tooltip("�����������")]
        public int enemyCount;
        [Tooltip("��ǰ��������")]
        public int currentEnemyCount;
        protected virtual void Awake()
        {
            AddWallCollider();
            currentEnemyCount = enemyCount;
            //�ϲ㴫����
            startPortal = CodeHelper.FindChild(transform, Consts.PrePortal);
            if (startPortal != null)
            {
                Portal p = startPortal.gameObject.AddComponent<Portal>();
                p.JumpIndex = roomIndex - 1;
            }

            //�²㴫����
            endPortal = CodeHelper.FindChild(transform, Consts.NextPortal);
            if (endPortal != null)
            {
                Portal p = endPortal.gameObject.AddComponent<Portal>();
                p.JumpIndex = roomIndex + 1;
            }
        }

        /// <summary>
        /// ��ǽ�����������ײ
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






