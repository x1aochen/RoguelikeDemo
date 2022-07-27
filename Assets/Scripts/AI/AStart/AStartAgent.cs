using System;
using System.Collections;
using UnityEngine;

namespace AI
{
    public class AStartAgent : MonoBehaviour
    {
        #region 寻路
        protected float speed = 20; //行进速度
        protected Vector3[] path;  //路径坐标
        protected int targetIndex; //当前所处路点下标
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
            //开始前进
            StartCoroutine(FollowPath());
        }

        /// <summary>
        /// 跟随路径向目标行进而去
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator FollowPath()
        {
            //从第一点开始
            //TODO: 如果巡逻点出现相同？
            if (path == null || path.Length <= 0)
                yield break;
            Vector3 currenWayPoint = path[0];
            while (targetIndex < path.Length)
            {
                //持续前进
                if (transform.position == currenWayPoint)
                {
                    targetIndex++;
                    //TODO:这边会越界?
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