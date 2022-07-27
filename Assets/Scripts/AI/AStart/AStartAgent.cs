using System;
using System.Collections;
using UnityEngine;

namespace AI
{
    public class AStartAgent : MonoBehaviour
    {
        #region Ѱ·
        protected float speed = 20; //�н��ٶ�
        protected Vector3[] path;  //·������
        protected int targetIndex; //��ǰ����·���±�
        protected float stopDistance;
        private PathFinding pathFinding;
        private void Awake()
        {
            pathFinding = CodeHelper.AddOrGetComponent<PathFinding>(gameObject);
        }

        public virtual void MoveToTarget(Vector3 targetPos,float speed)
        {
            this.speed = speed;
            pathFinding.StartFindPath(transform.position, targetPos, OnPathFound);
        }

        protected virtual void OnPathFound(Vector3[] newPath)
        {
            StopAllCoroutines();
            path = newPath;
            targetIndex = 0;
            //��ʼǰ��
            StartCoroutine(FollowPath());
        }

        /// <summary>
        /// ����·����Ŀ���н���ȥ
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator FollowPath()
        {
            //�ӵ�һ�㿪ʼ
            //TODO: ���Ѳ�ߵ������ͬ��
            if (path == null || path.Length <= 0)
                yield break;
            Vector3 currenWayPoint = path[0];
            while (targetIndex < path.Length)
            {
                //����ǰ��
                if (transform.position == currenWayPoint)
                {
                    targetIndex++;
                    //TODO:��߻�Խ��?
                    if (targetIndex <= 0 || targetIndex >= path.Length)
                        yield break;
                    currenWayPoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currenWayPoint, speed * Time.deltaTime);
                yield return null;
            }
        }
        #endregion

        public void StopFinding()
        {
            if (path == null || path.Length <= 0)
                return;
            StopAllCoroutines();
            targetIndex = 0;
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(path[i], Vector3.one * 0.5f);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}