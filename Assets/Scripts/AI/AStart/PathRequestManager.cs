using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// ���·��ʱ�Ĵ���
    /// </summary>
    public class PathRequestManager : MonoSingleton<PathRequestManager>
    {
        //����·�������Ƚ��ȳ�
        private Queue<PathRequest> pathRequests = new Queue<PathRequest>();
        //��ǰ·������
        private PathRequest currentRequest;
        private PathFinding pathFinding;
        //�Ƿ����ڴ���·��
        private bool isProcessingPath;

        public override void Init()
        {
            base.Init();
            //pathFinding = GetComponent<PathFinding>();
        }
        public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            //���������
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.pathRequests.Enqueue(newRequest);
            TryProcessNext();
        }

        /// <summary>
        /// ���Դ�����һ��·��
        /// </summary>
        private void TryProcessNext()
        {
            //�����ǰδ�ڹ����� ���� ���в�Ϊ��
            if (!isProcessingPath && pathRequests.Count > 0)
            {
                //���õ�ǰ����·��
                currentRequest = pathRequests.Dequeue();
                isProcessingPath = true;
                //��ʼ����Ѱ·
                //pathFinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
            }
        }

        /// <summary>
        /// ������Ѱ·��Ĵ���
        /// </summary>
        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            //���лص�����
            currentRequest.callback(path, success);
            //��������ִ��false
            isProcessingPath = false;
            //�������Խ�����һ��·���Ĵ���
            TryProcessNext();
        }
        /// <summary>
        /// ����·������������Ϣ
        /// </summary>
        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;
            public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
            {
                this.pathStart = start;
                this.pathEnd = end;
                this.callback = callback;
            }
        }
    }
}
